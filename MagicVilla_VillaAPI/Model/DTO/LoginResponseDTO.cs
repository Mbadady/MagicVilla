using System;
namespace MagicVilla_VillaAPI.Model.DTO
{
	public class LoginResponseDTO
	{
		public UserDTO User { get; set; }
		public string Token { get; set; }
		public string Roles { get; set; }
	}
}

