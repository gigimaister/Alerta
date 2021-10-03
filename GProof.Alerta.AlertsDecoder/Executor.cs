using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GProof.Alerta.AlertsDecoder.Entities;

namespace GProof.Alerta.AlertsDecoder
{
    internal  class Executor
    {
        public async Task Execute()
        {
            List<CityData> cities = await RetrieveCitiesData();
            ExcelHelper.ExcelHelper.CreateAndSaveExcelFile("Cities.xlsx", "Cities", cities);
        }

        private async Task<List<CityData>> RetrieveCitiesData()
        {
            Console.WriteLine("[+] Retrieving Cities From Json File");
            var dataRetriever = new DataRetriever();
            List<CityData> cities = dataRetriever.RetrieveCities();
            Console.WriteLine("[ OK ]\n");
            Console.WriteLine($"[+] Starting GET Web Request For {cities.Count} Cities");
            await dataRetriever.RetrieveCitiesAlarmData(cities);
            Console.WriteLine("[ OK ]\n");
            return cities;
        }
    }
}
