using System;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MagicVilla_Web.Services
{
	public class AuthService : BaseService, IAuthService
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private string _villaUrl;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
            : base(httpClientFactory)
		{
            _httpClientFactory = httpClientFactory;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:VillaApi");
		}

        public Task<T> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.POST,
                Url = _villaUrl + "/api/" + SD._version + "/UsersAuth/login",
                Data = loginRequestDTO
            });
        }

        public Task<T> RegisterAsync<T>(RegistrationDTO registrationDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = MagicVilla_Utility.SD.ApiType.POST,
                Url = _villaUrl + "/api/" + SD._version + "/UsersAuth/register",
                Data = registrationDTO
            });
        }
    }
}

