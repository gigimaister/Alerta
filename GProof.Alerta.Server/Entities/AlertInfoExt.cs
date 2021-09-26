using System;
using System.Collections.Generic;
using System.Linq;

namespace GProof.Alerta.Server.Entities
{
    public class AlertInfoExt : AlertInfo
    {
        public AlertInfoExt()
        {
        }

        public AlertInfoExt(AlertInfo alarmInfo, DateTime eventTime)
        {
            Id = alarmInfo.Id;
            Data = alarmInfo.Data;
            EventTime = eventTime;
        }

        public AlertInfoExt(int id, List<string> data, DateTime eventTime) : base(id, data)
        {
            EventTime = eventTime;
        }

        public DateTime EventTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            AlertInfoExt other = obj as AlertInfoExt;
            return Data == other.Data && EventTime == other.EventTime && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }

        public override string ToString()
        {
            string areas = string.Join(" ; ", Data.Select(x=> new string(x.Reverse().ToArray())));
            return $"Id: {Id}; Date: {EventTime:G} ; Areas: {areas}";
        }
    }
}
