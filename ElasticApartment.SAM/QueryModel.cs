using System.Collections.Generic;
using Newtonsoft.Json;

namespace ElasticApartment.SAM
{
    public class QueryModel
    {
        [JsonProperty("searchPhase")]
        public string SearchPhase { get; set; }
        
        [JsonProperty("market")]
        public List<string> Market { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}
