using System;
using AutoMapper;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.MappingConfig
{
	public class Mapping : Profile
	{
		public Mapping()
		{ 
			CreateMap<VillaCreateDTO, VillaDTO>().ReverseMap();
			CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();

	
			CreateMap<VillaNumberCreateDTO, VillaNumberDTO>().ReverseMap();
			CreateMap<VillaNumberUpdateDTO, VillaNumberDTO>().ReverseMap();


		}
    }
}

