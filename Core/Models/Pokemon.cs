using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class Pokemon
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }
        [JsonPropertyName("habitat")]
        public PokemonHabitat Habitat { get; set; }
        [JsonPropertyName("flavor_text_entries")]
        public List<FlavorTextEntries> FlovorTextEntries { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }

    }

    public class PokemonHabitat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class FlavorTextEntries
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; }
        [JsonPropertyName("language")]
        public Language Language { get; set; }
    }

    public class Language
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
