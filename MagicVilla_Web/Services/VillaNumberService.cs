using System;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_Web.Services
{
	public class VillaNumberService : BaseService, IVillaNumberService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private string _villaUrl;

		public VillaNumberService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            : base(httpClientFactory)
		{
            _httpClientFactory = httpClientFactory;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
		}

        public Task<T> CreateVillaNumberAsync<T>(VillaNumberCreateDTO createDTO, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
             ApiType = MagicVilla_Utility.SD.ApiType.POST,
             Url = _villaUrl + "/api/VillaNumberApi",
             Data = createDTO,
                Token = token
            });
        }

        public Task<T> DeleteVillaNumberAsync<T>(int villaNo, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.DELETE,
                Url = _villaUrl + "/api/VillaNumberApi/" + villaNo,
                Token = token
            });
        }

        public Task<T> GetAllVillaNumberAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.GET,
                Url = _villaUrl + "/api/VillaNumberApi",
                Token = token
            });
        }

        public Task<T> GetVillaNumberAsync<T>(int villaNo, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.GET,
                Url = _villaUrl + "/api/VillaNumberApi/" + villaNo,
                Token = token
            });
        }

        public Task<T> UpdateVillaNumberAsync<T>(VillaNumberUpdateDTO updateDTO, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.PUT,
                Url = _villaUrl + "/api/VillaNumberApi/" + updateDTO.VillaNo,
                Data = updateDTO,
                Token = token
            });
        }
    }
}

