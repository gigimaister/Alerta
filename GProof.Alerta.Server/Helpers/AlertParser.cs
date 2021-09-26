using System;
using System.Collections.Generic;
using GProof.Alerta.Server.Entities;
using Newtonsoft.Json;

namespace GProof.Alerta.Server.Helpers
{
    internal class AlertParser
    {
        public List<AlertInfo> Parse(string alarmsJson)
        {
            FileHelper.SaveAlarm(DateTime.Now.ToString("G") + ": " + alarmsJson);
            List<AlertInfo> alarms = null;
            try
            {
                alarms = JsonConvert.DeserializeObject<List<AlertInfo>>(alarmsJson);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{DateTime.Now} - Response: {alarmsJson}{Environment.NewLine}");
                FileHelper.SaveAlarm(e.Message);
                Console.WriteLine(e);
            }

            if (alarms != null && alarms.Count != 0)
            {
                return alarms;
            }

            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}{DateTime.Now} - Response: {alarmsJson}{Environment.NewLine}");
            Console.WriteLine(
                $"{Environment.NewLine}Fail to deserialize json response {Environment.NewLine}{Environment.NewLine}");

            return null;
        }

        private string GenerateTestAlarmJson()
        {
            return "[{\"data\": [\"אבשלום\",\"יבול\",\"שלומית\"],\"id\": 1631303468408,\"title\": \"התרעות פיקוד העורף\"}]";
        }
    }
}
