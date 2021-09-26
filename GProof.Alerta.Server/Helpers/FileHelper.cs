using System.IO;

namespace GProof.Alerta.Server.Helpers
{
    public static class FileHelper
    {
        private const string AlertsFile = "Alerts.txt";

        public static void SaveAlarm(string alertJson)
        {
            using StreamWriter sw = File.AppendText(AlertsFile);
            sw.WriteLine(alertJson);
        }
    }
}
