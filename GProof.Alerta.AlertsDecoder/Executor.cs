using System;
using System.Collections.Generic;
using GProof.Alerta.AlertsDecoder.Entities;

namespace GProof.Alerta.AlertsDecoder
{
    internal class Executor
    {
        public void Execute()
        {
            List<CityData> cities = RetrieveCitiesData();
            ExcelHelper.ExcelHelper.CreateAndSaveExcelFile("Cities.xlsx", "Cities", cities);
        }

        private List<CityData> RetrieveCitiesData()
        {
            var dataRetriever = new DataRetriever();
            List<CityData> cities = dataRetriever.RetrieveCities();
            return dataRetriever.RetrieveCitiesAlarmData(cities);
        }
    }
}
