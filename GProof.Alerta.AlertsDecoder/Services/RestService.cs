using GProof.Alerta.AlertsDecoder.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GProof.Alerta.AlertsDecoder.Services
{
    public static class RestService
    {
        #region GET
        //GET Pikud Hao'ref API Response(CityDataResponse) Containing Citie Information.

        public static async Task<CityDataResponse> GetCitiesAlarmDataNew(string url)
        {
            // Create an HttpClientHandler object and set to use default credentials
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            // Create an HttpClient object
            HttpClient client = new HttpClient(handler);

            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return new CityDataResponse();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            // Need to call dispose on the HttpClient and HttpClientHandler objects
            // when done using them, so the app doesn't leak resources
            handler.Dispose();
            client.Dispose();
            return null;
        }

        public static async Task<CityDataResponse> GetCitiesAlarmData(string cityid)
        {
            //For Https req
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, errors) => { return true; };

            HttpClient client = new HttpClient(httpClientHandler);
            StringContent queryString = new StringContent($"cityid={cityid}");

          
            var response = await client.GetAsync(Constants.PikudHaOrefUrl+cityid);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<CityDataResponse>(result);

                return json;
            }
            return null;
        }

        public static async Task<string> GetURI(string u)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", "Your Oauth token");
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }
        #endregion
    }
}
