using System.Text.Json.Serialization;

namespace Core.Models
{
    public class PokemonSpecies
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}