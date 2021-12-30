using Newtonsoft.Json;

namespace ElasticApartment.Model
{
    public class Management
    {
        [JsonProperty("mgmtID")]
        public int ManagementId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
