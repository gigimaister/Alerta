using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GProof.Alerta.AlertsDecoder.Entities;

namespace GProof.Alerta.AlertsDecoder
{
    internal  class Executor
    {
        public async Task Execute()
        {
            List<CityData> cities = await RetrieveCitiesData();
            ExcelHelper.ExcelHelper.CreateAndSaveExcelFile("..\\..\\..\\Cities.xlsx", "Cities", cities.Where(city=>city!=null).ToList());

            List<CityDataNotes> citiesDataNotes = cities.Select(city =>
            {
                if (city != null && city.Notes != null && city.Notes.Any())
                {
                    return city.Notes[0];
                }

                return null;
            }).Where(city => city != null).ToList();

            ExcelHelper.ExcelHelper.CreateAndSaveExcelFile("..\\..\\..\\CityDataNotes.xlsx", "Cities", citiesDataNotes);
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
