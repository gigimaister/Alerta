using Alerta.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alerta.Services
{
    public class RestService : IRestService
    {
        #region GET
        public async Task<T> Get<T>(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }

        //GET Gov API Response(Rootobject) Containing Cities List
        public async Task<Rootobject> GetAllCites(string url)
        {
            //For Https req
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            HttpClient client = new HttpClient(httpClientHandler);
            var response = await client.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Rootobject>(result);

                return json;
            }
            return null;
        }
        #endregion
    }
}
