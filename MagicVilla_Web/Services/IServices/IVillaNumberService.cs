using System;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
	public interface IVillaNumberService
	{
		Task<T> GetAllVillaNumberAsync<T>(string token);

		Task<T> GetVillaNumberAsync<T>(int villaNo, string token);

		Task<T> DeleteVillaNumberAsync<T>(int villaNo, string token);

		Task<T> CreateVillaNumberAsync<T>(VillaNumberCreateDTO createDTO, string token);

		Task<T> UpdateVillaNumberAsync<T>(VillaNumberUpdateDTO updateDTO, string token);

	}
}

