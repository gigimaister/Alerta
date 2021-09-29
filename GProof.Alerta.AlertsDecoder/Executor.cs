using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GProof.Alerta.AlertsDecoder.Entities;

namespace GProof.Alerta.AlertsDecoder
{
    internal class Executor
    {
        public async Task Execute()
        {
            List<CityData> cities = await RetrieveCitiesData();
            ExcelHelper.ExcelHelper.CreateAndSaveExcelFile("Cities.xlsx", "Cities", cities);
        }

        private async Task<List<CityData>> RetrieveCitiesData()
        {
            var dataRetriever = new DataRetriever();
            List<CityData> cities = dataRetriever.RetrieveCities();
            await dataRetriever.RetrieveCitiesAlarmData(cities);
            return cities;
        }
    }
}
