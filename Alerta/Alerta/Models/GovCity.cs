using System.Collections.ObjectModel;

namespace Alerta.Models
{
    public class Rootobject
    {
        public string help { get; set; }
        public bool success { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public bool include_total { get; set; }
        public int limit { get; set; }
        public string records_format { get; set; }
        public string resource_id { get; set; }
        public object total_estimation_threshold { get; set; }
        public ObservableCollection<GovCity> records { get; set; }
        public Field[] fields { get; set; }
        public _Links _links { get; set; }
    }

    public class _Links
    {
        public string start { get; set; }
        public string next { get; set; }
    }

    public class GovCity
    {
        public int _id { get; set; }
        public string טבלה { get; set; }
        public int סמל_ישוב { get; set; }
        public string שם_ישוב { get; set; }
        public string שם_ישוב_לועזי { get; set; }
        public int סמל_נפה { get; set; }
        public string שם_נפה { get; set; }
        public int סמל_לשכת_מנא { get; set; }
        public string לשכה { get; set; }
        public int סמל_מועצה_איזורית { get; set; }
        public string שם_מועצה { get; set; }
    }

    public class Field
    {
        public string id { get; set; }
        public string type { get; set; }
    }
}