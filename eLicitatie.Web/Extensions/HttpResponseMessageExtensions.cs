using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace eLicitatie.Web.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static T ContentAsType<T>(this HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            string data = response.Content.ReadAsStringAsync().Result;
            return data.IsNullEmptyOrWhiteSpace() ? default : JsonConvert.DeserializeObject<T>(data);
        }

        public static async Task<T> ContentAsTypeAsync<T>(this HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            string data = await response.Content.ReadAsStringAsync();
            if (data.IsNullOrWhiteSpace())
            {
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (JsonSerializationException)
            {
                throw;
            }
        }
    }
}