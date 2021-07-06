using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Core;
using Core.Models;
using Microsoft.Extensions.Configuration;

namespace DataRepository.Pokemons
{
    public class PokemonDataRepository : IPokemonDataRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _webRepositoryUrl;

        public PokemonDataRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _webRepositoryUrl = configuration.GetValue<string>("Pokemons:DataSourceUrl");
        }

        public async Task<IEnumerable<string>> ReadAsync(int count)
        {
            string limitedResultsUrl = $"{_webRepositoryUrl}?limit={count}";
            using var response = await _httpClient.GetAsync(limitedResultsUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content?.ReadAsStringAsync();
            var pokemonSpeciesResults = JsonSerializer.Deserialize<PokemonSpeciesResults>(content);
            return pokemonSpeciesResults.Names;
        }

        public async Task<Pokemon> ReadAsync(string name)
        {
            string escapedName = Uri.EscapeDataString(name);
            string getByNameUrl = $"{_webRepositoryUrl}/{escapedName}";
            using var response = await _httpClient.GetAsync(getByNameUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            string content = await response.Content?.ReadAsStringAsync();
            var pokemonSpeciesResult = JsonSerializer.Deserialize<Pokemon>(content);

            return pokemonSpeciesResult;
        }

    }
}
