using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class Pokedex
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("isLegendary")]
        public bool IsLegendary { get; set; }
        [JsonPropertyName("habitat")]
        public string Habitat { get; set; }
        public TranslationType TranslationType { get; set; }

        public Pokedex(Pokemon pokemon)
        {
            Name = pokemon.Name;
            Description = pokemon.FlovorTextEntries.FirstOrDefault(f => "en".Equals(f.Language?.Name, StringComparison.InvariantCultureIgnoreCase))?.FlavorText;
            IsLegendary = pokemon.IsLegendary;
            Habitat = pokemon.Habitat?.Name;
            TranslationType = TranslationType.None;
        }
    }
}
