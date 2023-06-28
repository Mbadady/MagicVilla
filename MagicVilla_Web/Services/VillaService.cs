using System;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_Web.Services
{
	public class VillaService : BaseService, IVillaService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private string _villaUrl;

		public VillaService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            : base(httpClientFactory)
		{
            _httpClientFactory = httpClientFactory;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
		}

        public Task<T> CreateVillaAsync<T>(VillaCreateDTO createDTO, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
             ApiType = MagicVilla_Utility.SD.ApiType.POST,
             Url = _villaUrl + "/api/" + SD._version + "/VillaApi",
             Data = createDTO,
             Token = token
            });
        }

        public Task<T> DeleteVillaAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.DELETE,
                Url = _villaUrl + "/api/" + SD._version + "/VillaApi/" + id,
            Token = token
            });
        }

        public Task<T> GetAllVillaAsync<T>(string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = _villaUrl + "/api/" + SD._version + "/VillaApi",
               Token = token
            });
        }

        public Task<T> GetVillaAsync<T>(int id, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = _villaUrl + "/api/" + SD._version + "/VillaApi/" + id,
                Token = token
            });
        }

        public Task<T> UpdateVillaAsync<T>(VillaUpdateDTO updateDTO, string token)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Url = _villaUrl + "/api/" + SD._version + "/VillaApi/" + updateDTO.Id,
                Data = updateDTO,
                Token = token
            });
        }
    }
}

