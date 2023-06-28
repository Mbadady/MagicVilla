using System;
using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI.DataStore
{
	public static class DataStore
	{
		public static List<VillaDTO> VillaStore = new List<VillaDTO>
		{
			new VillaDTO{Id = 1, Name = "Pool View", Sqft = 200, Occupancy = 2},
			new VillaDTO{Id = 2, Name = "Ocean View", Sqft = 300, Occupancy = 1}
		};
	}
		

	}


