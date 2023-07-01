using System.Text;
using MagicVilla_VillaAPI.DataStore;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.MappingConfig;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

//using Serilog;

var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
// Add services to the container.

// This is for Serilog
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.File("log/villa.log", rollingInterval: RollingInterval.Day)
//    .CreateLogger();
//builder.Host.UseSerilog();

// for custom logging
//builder.Services.AddSingleton<ILogging, Logging>();

// AddNewtonsoftJson is for patching
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30", new CacheProfile()
    {
        Duration = 30
    });
}).AddNewtonsoftJson();
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
    option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddScoped<IVillaRepository, VillaRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = "v1",
        Title = "Magic Villa V1",
        Description = "Api to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Email = "victorsomtochukwu@gmail.com",
            Name = "Mbah Victor Somtochukwu",
            Url = new Uri("https://mbadady.me")
        }
    });

    options.SwaggerDoc("v2", new OpenApiInfo()
    {
        Version = "v2",
        Title = "Magic Villa V2",
        Description = "Api to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Email = "victorsomtochukwu@gmail.com",
            Name = "Mbah Victor Somtochukwu",
            Url = new Uri("https://mbadady.me")
        }
    });

});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAutoMapper(typeof(Mapping));

builder.Services.AddResponseCaching();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

