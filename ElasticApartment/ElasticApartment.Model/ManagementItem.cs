using Newtonsoft.Json;

namespace ElasticApartment.Model
{
    public class ManagementItem
    {
        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }
}
