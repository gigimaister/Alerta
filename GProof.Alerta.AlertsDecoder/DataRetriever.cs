using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GProof.Alerta.AlertsDecoder.Entities;
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


        public void RetrieveCitiesAlarmData(List<CityData> cities)
        {
            
        }
    }
}
