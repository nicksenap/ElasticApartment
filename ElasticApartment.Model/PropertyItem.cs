using Newtonsoft.Json;

namespace ElasticApartment.Model
{
    public class PropertyItem
    {
        [JsonProperty("property")]
        public Property Property { get; set; }
    }
}
