using System;
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

        public Task<T> CreateVillaAsync<T>(VillaCreateDTO createDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
             ApiType = MagicVilla_Utility.SD.ApiType.POST,
             Url = _villaUrl + "/api/VillaApi",
             Data = createDTO
            });
        }

        public Task<T> DeleteVillaAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.DELETE,
                Url = _villaUrl + "/api/VillaApi/" + id,
            
            });
        }

        public Task<T> GetAllVillaAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.GET,
                Url = _villaUrl + "/api/VillaApi",
               
            });
        }

        public Task<T> GetVillaAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.GET,
                Url = _villaUrl + "/api/VillaApi/" + id,
            });
        }

        public Task<T> UpdateVillaAsync<T>(VillaUpdateDTO updateDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.PUT,
                Url = _villaUrl + "/api/VillaApi/" + updateDTO.Id,
                Data = updateDTO
            });
        }
    }
}

