using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            //Poplulate pikudCities.json To C# Models.PikudCity
            var cityDatalist = JsonConvert.DeserializeObject<List<CityData>>
                (File.ReadAllText(@"C:\Users\koni\source\repos\gigimaister\Alerta\GProof.Alerta.AlertsDecoder\Resources\pikudcities.json", Encoding.Default));
            return cityDatalist;
        }


        public async Task RetrieveCitiesAlarmData(List<CityData> cities)
        {
            //Populate each Navigation Property(CityDataNotes) For Each Object
            foreach (var city in cities)
            {
                try
                {
                    var response = await RestService.GetCitiesAlarmData(Urls.GetAllLocations + city.CityId);
                    city.CitydataNotes.AreaCode = response.Notes.Select(x => x.AreaCode).FirstOrDefault();
                    city.CitydataNotes.AreaId = response.Notes.Select(x => x.AreaId).FirstOrDefault();
                    city.CitydataNotes.AreaRange = response.Notes.Select(x => x.AreaRange).FirstOrDefault();
                    city.CitydataNotes.AreaRangeSecond = response.Notes.Select(x => x.AreaRangeSecond).FirstOrDefault();
                    city.CitydataNotes.CityName = response.Notes.Select(x => x.CityName).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }   
             
        }
    }
}
