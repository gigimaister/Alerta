using System.Text;

namespace GProof.Alerta.AlertsDecoder
{
    internal static class Constants
    {

        public readonly static Encoding HebrewEncoding = Encoding.GetEncoding("Windows-1255");

        public const string CitiesJsonFilePath = "..\\..\\..\\Resources\\pikudcities.json";

        public const string PikudHaOrefUrl = "https://www.oref.org.il/Shared/Ajax/GetAlarmInstructions.aspx?lang=he&from=1&";
    }
}
