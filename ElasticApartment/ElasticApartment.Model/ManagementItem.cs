using Newtonsoft.Json;

namespace ElasticApartment.Model.Models
{
    public class ManagementItem
    {
        [JsonProperty("mgmt")]
        public Management Management { get; set; }
    }
}
