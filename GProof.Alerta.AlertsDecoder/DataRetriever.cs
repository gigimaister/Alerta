using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GProof.Alerta.AlertsDecoder.Entities;
using GProof.Alerta.AlertsDecoder.Services;
using Newtonsoft.Json;

namespace GProof.Alerta.AlertsDecoder
{
    internal class DataRetriever
    {
       
        public List<CityData> RetrieveCities()
        {
            string cistiesJson = File.ReadAllText(Constants.CitiesJsonFilePath, Constants.HebrewEncoding);
            //Poplulate pikudCities.json To C# Models.PikudCity
            var cityDatalist = JsonConvert.DeserializeObject<List<CityData>>(cistiesJson);
            // (File.ReadAllText(@"C:\Users\koni\source\repos\gigimaister\Alerta\GProof.Alerta.AlertsDecoder\Resources\pikudcities.json", Encoding.Default));

            return cityDatalist;
        }


        public async Task RetrieveCitiesAlarmData(List<CityData> cities)
        {
            //Populate each Navigation Property(CityDataNotes) For Each Object
            foreach (var city in cities)
            {
                try
                {
                    string data =  RetrieveCityData(city.CityId);
                    var cityDataNote = JsonConvert.DeserializeObject<CityDataResponse>(data);
                    try
                    {
                        city.CitydataNotes = cityDataNote.Notes;

                    }
                    catch(SystemException ex)
                    {
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }   
             
        }

        public string RetrieveCityData(string cityid)
        {
            try
            {
                WebRequest pikodHaOrefClient = GetPikodHaOrefClient(cityid);
                using Stream s = pikodHaOrefClient.GetResponse().GetResponseStream();
                using StreamReader sr = new StreamReader(s);
                return sr.ReadToEnd();
            }
            catch (Exception e)
            {
            }

            return null;
        }

        private WebRequest GetPikodHaOrefClient(string cityId)
        {
            string url = $"{Constants.PikudHaOrefUrl}cityid={cityId}";
            var webRequest = WebRequest.Create(url);
            webRequest.Method = "GET";
            webRequest.Timeout = 12000;
            webRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            webRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            webRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            webRequest.Headers.Add("Referer", "https://www.oref.org.il/11088-13708-he/Pakar.aspx");
            webRequest.Headers.Add("sec-ch-ua",
                "\"Not A;Brand\";v=\"99\", \"Chromium\";v=\"90\", \"Google Chrome\";v=\"90\"");
            webRequest.Headers.Add("sec-ch-ua-mobile", "?0");
            webRequest.Headers.Add("User-Agent", "");
            return webRequest;
        }
        static async Task<string> GetURI(Uri u)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }
    }
}

