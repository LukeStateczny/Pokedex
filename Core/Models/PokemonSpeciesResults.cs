using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class PokemonSpeciesResults
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("next")]
        public string Next { get; set; }
        [JsonPropertyName("previous")]
        public string Previous { get; set; }
        [JsonPropertyName("results")]
        public List<PokemonSpecies> Results { get; set; }
        public IEnumerable<string> Names => Results.Select(ps => ps.Name);
    }
}
