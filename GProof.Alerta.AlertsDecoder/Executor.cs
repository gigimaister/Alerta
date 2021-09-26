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

        }

        private List<CityData> RetrieveCitiesData()
        {
            throw new NotImplementedException();
        }


    }
}
