using Newtonsoft.Json;

namespace ElasticApartment.Model
{
    public class Property
    {
        [JsonProperty("propertyID")]
        public int PropertyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("formerName")]
        public string FormerName { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}