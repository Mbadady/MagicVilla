using System;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
	public interface IUserRepository
	{
		bool isUnique(string username);

		Task<LoginResponseDTO> Login(LoginRequestDTO login);

		Task<LocalUser> Register(RegistrationDTO registration);
	}
}

