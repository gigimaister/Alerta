using System.Collections.Generic;
using Newtonsoft.Json;

namespace GProof.Alerta.Server.Entities
{
//    8/16/2021 1:32:10 PM - Response: {
//    "data": [
//    "?????, ?????, ??? ??",
//    "???",
//    "????? ??? ??"
//    ],
//    "id": 1629109926185,
//    "title": "?????? ????? ?????"
//}

//    [{
//    "data": [
//        "אבשלום",
//        "יבול",
//        "שלומית"
//    ],
//    "id": 1631303468408,
//    "title": "התרעות פיקוד העורף"
//}]
    public class AlertInfo
    {
        public AlertInfo()
        {
        }

        public AlertInfo(int id, List<string> data)
        {
            Id = id;
            Data = data;
        }
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("data")]
        public List<string> Data { get; set; }
    }
}
