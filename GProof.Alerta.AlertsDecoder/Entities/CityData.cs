namespace GProof.Alerta.AlertsDecoder.Entities
{
    //Final Object
    public class CityData
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Id { get; set; }
        public string Aareaid { get; set; }
        public string Mixname { get; set; }
        public string Color { get; set; }
        public string Code { get; set; }
        public string Area { get; set; }
        public CityDataNotes  CitydataNotes { get; set; }
    }
    //After Sending GET req To Pikud Hao'ref API With City Id We Get
    //CityDataResponse(Time To Go The Shelter + More Info)
    public class CityDataResponse
    {
        public CityDataNotes[] Notes { get; set; }
        public City[] City { get; set; }
    }

    public class CityDataNotes
    {
        public string Code { get; set; }
        public string Heb_name { get; set; }
        public string Area_name { get; set; }
        public int Area_id { get; set; }
        public string Zone_name { get; set; }
        public string Zone_name1 { get; set; }
        public string Time_notes { get; set; }
    }

    public class City
    {
        public string Cityname { get; set; }
        public int Houserows { get; set; }
    }

}
