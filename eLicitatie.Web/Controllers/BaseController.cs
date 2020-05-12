using eLicitatie.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eLicitatie.Web.Controllers
{
    public class BaseController : Controller
    {
        protected HttpClient HttpClient;

        public BaseController(IConfiguration configuration)
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(configuration["ApiUrl"].ToString(), UriKind.Absolute)
            };
        }

        protected async Task<HttpResponseMessage> GetAsync(string url, bool anonymous = false)
        {
            SetTokenOnRequest(anonymous);

            return await HttpClient.GetAsync(url);
        }

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T model, bool anonymous = false)
        {
            SetTokenOnRequest(anonymous);

            return await HttpClient.PostAsJsonAsync(url, model);
        }

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T model, bool anonymous = false)
        {
            SetTokenOnRequest(anonymous);

            return await HttpClient.PutAsJsonAsync<T>(url, model);
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string url, bool anonymous = false)
        {
            SetTokenOnRequest(anonymous);

            return await HttpClient.DeleteAsync(url);
        }

        private void SetTokenOnRequest(bool anonymous = false)
        {
            if (!anonymous)
            {
                Claim token = User.FindFirst("access_token");
                if (token != null && !token.Value.IsNullEmptyOrWhiteSpace())
                {
                    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
                }
            }
            else
            {
                HttpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}