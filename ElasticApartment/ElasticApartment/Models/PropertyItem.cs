using Newtonsoft.Json;

namespace ElasticApartment.Models
{
    public class PropertyItem
    {
        [JsonProperty("property")]
        public Property Property { get; set; }
    }
}
