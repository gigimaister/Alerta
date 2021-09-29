using GProof.Alerta.AlertsDecoder.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GProof.Alerta.AlertsDecoder.Services
{
    public static class RestService
    {
        #region GET
        //GET Pikud Hao'ref API Response(CityDataResponse) Containing Citie Information.
        public static async Task<CityDataResponse> GetCitiesAlarmData(string url)
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
                var json = JsonConvert.DeserializeObject<CityDataResponse>(result);

                return json;
            }                     
            return null;
        }
        #endregion
    }
}
