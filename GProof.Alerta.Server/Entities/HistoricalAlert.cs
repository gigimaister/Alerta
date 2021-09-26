using System;
using System.Linq;

namespace GProof.Alerta.Server.Entities
{
    public class HistoricalAlert
    {
        public string Data { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public DateTime Datetime { get; set; }

        public override string ToString()
        {
            var area = Data.Reverse().ToArray();
            return $"Received: {System.DateTime.Now:G} ; Date: {Datetime:G} ; Time:{Time} - Area: {new string(area)}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            HistoricalAlert other = obj as HistoricalAlert;
            return Data == other.Data && Date == other.Date & Time == other.Time;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
