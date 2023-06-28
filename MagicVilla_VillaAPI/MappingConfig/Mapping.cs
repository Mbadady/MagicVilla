using System;
using AutoMapper;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;

namespace MagicVilla_VillaAPI.MappingConfig
{
	public class Mapping : Profile
	{
		public Mapping()
		{
			CreateMap<Villa, VillaDTO>();
			CreateMap<VillaDTO, Villa>();


			CreateMap<VillaCreateDTO, Villa>().ReverseMap();
			CreateMap<VillaUpdateDTO, Villa>().ReverseMap();

			CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
			CreateMap<VillaNumberCreateDTO, VillaNumber>().ReverseMap();
			CreateMap<VillaNumberUpdateDTO, VillaNumber>().ReverseMap();


		}
    }
}

