using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MagicVilla_VillaAPI.DataStore;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly string _secretKey;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration
            , UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public bool isUnique(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO login)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == login.Username.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, login.Password);

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            //Assign role to the user
            var roles = await _userManager.GetRolesAsync(user);
            // One way to generate Token
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),

                Expires = DateTime.UtcNow.AddDays(7),

                SigningCredentials = new(key, SecurityAlgorithms.HmacSha256Signature)
            };

            //This returns SecurityToken as a return type
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //This now is serialized to a jwt string
            var jwt = tokenHandler.WriteToken(token);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = jwt,
                //User = new UserDTO()
                //{
                //    ID = user.Id,
                //    Name = user.Name,
                //    Username = user.Email   
                //},
                User = _mapper.Map<UserDTO>(user),

                Roles = roles.FirstOrDefault()
            };

            //Another easier way to generate Token

            //List<Claim> claims = new List<Claim>()
            //{
            //    new Claim(ClaimTypes.Name, user.Username),
            //    new Claim(ClaimTypes.Role, user.Role)
            //};

            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
            ////OR
            ////var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //var expires = DateTime.Now.AddDays(7);

            //var token = new JwtSecurityToken(
            //    signingCredentials: creds,
            //    expires: expires,
            //    claims: claims
            //    );
            //var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            //LoginResponseDTO loginResponseDTO = new()
            //{
            //    Token = jwt,
            //    User = user
            //};

            return loginResponseDTO;
        }

        public async Task<UserDTO> Register(RegistrationDTO registration)
        {
            ApplicationUser user = new()
            {
                Name = registration.Name,
                Email = registration.Username,
                UserName = registration.Username,
                NormalizedEmail = registration.Username.ToUpper()
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registration.Password);

                //if (registration.Password.Length < 6)
                //{
                //    var errors = result.Errors.Select(e => e.Description);
                //}

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, registration.Role);

                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registration.Username);

                    return _mapper.Map<UserDTO>(userToReturn);
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return new UserDTO();
        }
    }
}