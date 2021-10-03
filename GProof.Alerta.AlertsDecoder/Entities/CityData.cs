using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GProof.Alerta.AlertsDecoder.Entities
{
    //Final Object
    public class CityData
    {
        [JsonProperty("label")]
        public string CityName { get; set; }

        [JsonProperty("value")]
        public string CityValue { get; set; }

        [JsonProperty("id")]
        public string CityId { get; set; }

        [JsonProperty("areaid")]
        public string AreaId { get; set; }

        [JsonProperty("mixname")]
        public string CityFullName { get; set; }

        [JsonProperty("color")]
        public string AreaAlertColor { get; set; }

        [JsonProperty("code")]
        public string AreaCode { get; set; }

        [JsonProperty("area")]
        public string GlobalAreaName { get; set; }

        [JsonIgnore]
        public string NotesAsString => Notes == null || !Notes.Any()
            ? ""
            : $"AreaId: {Notes[0].AreaId} ; AreaCode: {Notes[0].AreaCode} ; AreaRange: {Notes[0].AreaRange}; AreaRangeSecond: {Notes[0].AreaRangeSecond} ; CityName: {Notes[0].CityName} ; GlobalAreaName: {Notes[0].GlobalAreaName} ; TimeForShelterCover: {Notes[0].TimeForShelterCover} ;";

        [JsonProperty("CitydataNotes")]
        public List<CityDataNotes> Notes { get; set; }
    }
    //After Sending GET req To Pikud Hao'ref API With City Id We Get
    //CityDataResponse(Time To Go The Shelter + More Info)
    public class CityDataResponse
    {
        public List<CityDataNotes> Notes { get; set; }
        public City[] City { get; set; }
    }

    public class CityDataNotes
    {
        [JsonProperty("code")]
        public string AreaCode { get; set; }

        [JsonProperty("heb_name")]
        public string CityName { get; set; }

        [JsonProperty("area")]
        public string GlobalAreaName { get; set; }

        [JsonProperty("areaid")]
        public string AreaId { get; set; }

        [JsonProperty("zone_name")]
        public string AreaRange { get; set; }

        [JsonProperty("zone_name1")]
        public string AreaRangeSecond { get; set; }

        [JsonProperty("time_notes")]
        public string TimeForShelterCover { get; set; }
    }

    public class City
    {
        public string Cityname { get; set; }
        public int Houserows { get; set; }
    }

}
