using System;
namespace MagicVilla_VillaAPI.Model.DTO
{
	public class LoginResponseDTO
	{
		public LocalUser User { get; set; }
		public string Token { get; set; }
	}
}

