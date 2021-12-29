using Newtonsoft.Json;

namespace ElasticApartment.Model.Models
{
    public class PropertyItem
    {
        [JsonProperty("property")]
        public Property Property { get; set; }
    }
}
