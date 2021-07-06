using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Models;
using DataRepository.Pokemons;
using DataRepository.Translations;

namespace Core.BusinessLogic
{
    public class PokemonService
    {
        private readonly IPokemonDataRepository _pokemonRepository;

        public PokemonService(IPokemonDataRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }


        public async Task<Pokemon> Read(string idOrName)
        {
            var pokemon = await _pokemonRepository.ReadAsync(idOrName);
            return pokemon;
        }

        public async Task<Pokedex> Convert(Pokemon pokemon, IDescriptionTranslationRepository translatorRepo)
        {
            var pokedex = Convert(pokemon);
            var translationType = GetTranslationType(pokemon);
            try
            {
                var translatedDescription = await translatorRepo.TranslateAsync(pokedex.Description, translationType);

                pokedex.TranslationType = translationType;
                pokedex.Description = translatedDescription;
            }
            catch
            {
                // We bypass throttling and other errors ;) for now
            }
            return pokedex;
        }

        public Pokedex Convert(Pokemon pokemon)
        {
            return new Pokedex(pokemon);
        }

        private TranslationType GetTranslationType(Pokemon pokemon)
        {
            TranslationType translationType = TranslationType.None;
            string habitat = "cave";

            if (habitat.Equals(pokemon.Habitat?.Name, StringComparison.InvariantCultureIgnoreCase)// if habitat is 'cave'
                || pokemon.IsLegendary)    // or if is 'legendary'
            {
                translationType = TranslationType.Yoda;
            }
            else
            {
                translationType = TranslationType.Shakespeare;
            }
            return translationType;
        }

        public async Task<IEnumerable<string>> List(int limit)
        {
            var res = await _pokemonRepository.ReadAsync(limit);
            return res;
        }
    }
}
