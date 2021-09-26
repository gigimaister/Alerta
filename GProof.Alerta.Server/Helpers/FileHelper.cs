
using System.IO;

namespace GProof.Alerta.Server.Helpers
{
    public static class FileHelper
    {
        private const string AlarmsFile = "Alarms.txt";

        public static void SaveAlarm(string alarmJson)
        {
            using StreamWriter sw = File.AppendText(AlarmsFile);
            sw.WriteLine(alarmJson);
        }
    }
}
