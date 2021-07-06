using System;
using System.IO;
using System.Reflection;
using Core.Models;
using Core.Parsers;
using NUnit.Framework;
namespace WebApi.Tests
{
    public class ParsersTests
    {

        [Test]
        public void DeserializePokemonSpeciesTest()
        {
            var jsonFile = Path.Combine(AppContext.BaseDirectory, "Data/Responses/PokemonSpeciesResults.json");
            string jsonString = File.ReadAllText(jsonFile);
            bool res = PokemonParsers.TryParsePokemonSpecies(jsonString, out PokemonSpeciesResults _);
            Assert.IsTrue(res);
        }

        [Test]
        public void DeserializePokemonTest()
        {
            var jsonFile = Path.Combine(AppContext.BaseDirectory, "Data/Responses/PokemonSpeciesWormedam.json");
            string jsonString = File.ReadAllText(jsonFile);
            bool res = PokemonParsers.TryParsePokemon(jsonString, out Pokemon _);
            Assert.IsTrue(res);
        }

        [Test]
        public void DeserializeTranslationTest()
        {
            var jsonFile = Path.Combine(AppContext.BaseDirectory, "Data/Responses/Translation.json");
            string jsonString = File.ReadAllText(jsonFile);
            bool res = TranslationParsers.TryParseTranslation(jsonString, out Translation _);
            Assert.IsTrue(res);
        }
    }
}
