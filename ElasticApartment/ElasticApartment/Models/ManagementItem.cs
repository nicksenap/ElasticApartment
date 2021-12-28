using Newtonsoft.Json;

namespace ElasticApartment.Models
{
    public class ManagementItem
    {
        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }
}
