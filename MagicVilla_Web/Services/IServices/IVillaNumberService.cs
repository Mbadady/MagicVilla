﻿using System;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
	public interface IVillaService
	{
		Task<T> GetAllVillaAsync<T>();
		Task<T> GetVillaAsync<T>(int id);
		Task<T> DeleteVillaAsync<T>(int id);
		Task<T> CreateVillaAsync<T>(VillaCreateDTO createDTO);
		Task<T> UpdateVillaAsync<T>(VillaUpdateDTO updateDTO);

	}
}

