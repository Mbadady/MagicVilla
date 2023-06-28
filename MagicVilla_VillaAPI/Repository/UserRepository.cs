using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MagicVilla_VillaAPI.DataStore;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla_VillaAPI.Repository
{
	public class UserRepository : IUserRepository
	{
        private readonly ApplicationDbContext _db;
        private readonly string _secretKey;
		public UserRepository(ApplicationDbContext db, IConfiguration configuration)
		{
            _db = db;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
		}

        public bool isUnique(string username)
        {
           var user = _db.LocalUsers.FirstOrDefault(u => u.Username == username);

            if(user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO login)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.Username.ToLower() == login.Username.ToLower() && u.Password == login.Password);

            if(user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            // One way to generate Token
            //var tokenHandler = new JwtSecurityTokenHandler();

            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));

            //var tokenDescriptor = new SecurityTokenDescriptor()
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, user.Username),
            //        new Claim(ClaimTypes.Role, user.Role)
            //    }),

            //    Expires = DateTime.UtcNow.AddDays(7),

            //    SigningCredentials = new(key, SecurityAlgorithms.HmacSha256Signature)
            //};

            // This returns SecurityToken as a return type
            //var token = tokenHandler.CreateToken(tokenDescriptor);

            // This now is serialized to a jwt string 
            // var jwt = tokenHandler.WriteToken(token)

            //LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            //{
            //    Token = jwt,
            //    User = user
            //};

            //Another easier way to generate Token

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
            //OR
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var expires = DateTime.Now.AddDays(7);

            var token = new JwtSecurityToken(
                signingCredentials: creds,
                expires: expires,
                claims: claims
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            LoginResponseDTO loginResponseDTO = new()
            {
                Token = jwt,
                User = user
            };

            loginResponseDTO.User.Password = "";
            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegistrationDTO registration)
        {
            LocalUser user = new()
            {
                Name = registration.Name,
                Password = registration.Password,
                Username = registration.Username,
                Role = registration.Role
            };

            await _db.LocalUsers.AddAsync(user);
            await _db.SaveChangesAsync();

            user.Password = "";

            return user;
        }
    }
}

