using Newtonsoft.Json;

namespace ElasticApartment.Models
{
    public class QueryModel
    {
        [JsonProperty("searchPhase")]
        public string SearchPhase { get; set; }
        
        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}
