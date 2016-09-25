using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Waes.Diffly.IntegrationTest.Extension
{
    public static class GeneralExtension
    {
        /// <summary>
        /// Generates a HttpContent with JSON serialized value.
        /// </summary>
        public static HttpContent ToJsonHttpContent(this object value)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(value));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }

        public static async Task<T> ToDto<T>(this HttpResponseMessage httpResponseMessage) where T: class
        {
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}
