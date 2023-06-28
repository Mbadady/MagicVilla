using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseServices
    {
        public APIResponse ResponseModel { get ; set ; }
        public IHttpClientFactory HttpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.HttpClient = httpClient;
            this.ResponseModel = new APIResponse();
        }

        public async Task<T> SendAsync<T>(APIRequest request)
        {
            try
            {


                var client = HttpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();

                message.Headers.Add("Accept", "Application/json");
                message.RequestUri = new Uri(request.Url);

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
                switch (request.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage response = null;

                // When we are calling the API, we are passing the token
                if (!string.IsNullOrEmpty(request.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.Token);
                }
                response = await client.SendAsync(message);

                var apiContent = await response.Content.ReadAsStringAsync();

                //var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                try
                {
                    APIResponse apiResponse = JsonConvert.DeserializeObject<APIResponse>(apiContent);

                    if(apiResponse ==null || response.StatusCode == System.Net.HttpStatusCode.BadRequest
                        || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        apiResponse.IsSuccess = false;

                        var res = JsonConvert.SerializeObject(apiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);

                        return returnObj;
                    }
                    else
                    {
                        var returnedAPIResponse = JsonConvert.DeserializeObject<T>(apiContent);

                        return returnedAPIResponse;

                    }
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);

                    return exceptionResponse;

                }

                

            } catch(Exception ex)
            {
                var dto = new APIResponse()
                {
                    ErrorMessages = new List<string>() { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var returnedAPIResponse = JsonConvert.DeserializeObject<T>(res);

                return returnedAPIResponse;
            }
        }
    }
}

