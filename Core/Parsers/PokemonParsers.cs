using System;
using System.Text.Json;
using Core.Models;

namespace Core.Parsers
{
    public class PokemonParsers
    {
        static JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static bool TryParsePokemon(string json, out Pokemon pokemon)
        {
            pokemon = JsonSerializer.Deserialize<Pokemon>(json, serializeOptions);
            return pokemon != null;
        }

        public static bool TryParsePokemonSpecies(string json, out PokemonSpeciesResults pokemonSpeciesResults)
        {
            pokemonSpeciesResults = JsonSerializer.Deserialize<PokemonSpeciesResults>(json, serializeOptions);
            return pokemonSpeciesResults != null;
        }

    }
}
