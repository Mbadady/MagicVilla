using System;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
	public interface IVillaService
	{
		Task<T> GetAllVillaAsync<T>(string token);
		Task<T> GetVillaAsync<T>(int id, string token);
		Task<T> DeleteVillaAsync<T>(int id, string token);
		Task<T> CreateVillaAsync<T>(VillaCreateDTO createDTO, string token);
		Task<T> UpdateVillaAsync<T>(VillaUpdateDTO updateDTO, string token);

	}
}

