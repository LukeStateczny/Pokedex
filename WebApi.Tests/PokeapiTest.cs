using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Models;
using DataRepository;
using DataRepository.Pokemons;
using DataRepository.Translations;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using WebApi.Controllers;
using Core.BusinessLogic;
using FluentAssertions;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Tests
{
    [Category("pokeapi")]
    public class WebApiControllerTest
    {
        IPokemonDataRepository _fakeDataStore;
        IDescriptionTranslationRepository _translatorRepo;
        ILogger<PokemonsController> _logger;

        [SetUp]
        public void Setup()
        {
            _fakeDataStore = A.Fake<IPokemonDataRepository>();
        }

        [Test]
        public async Task GivenCountProvided_WhenGetPokemons_ThenReturnsTheCorrectNumberOfItems()
        {
            int count = 5;
            var fakeData = A.CollectionOfDummy<string>(count).AsEnumerable();

            A.CallTo(() => _fakeDataStore.ReadAsync(count)).Returns(Task.FromResult(fakeData));
            var controller = new PokemonsController(_fakeDataStore, _translatorRepo, _logger);
            var controllerResult = await controller.GetPokemonsV1(count);
            var objectResult = controllerResult.Result as OkObjectResult;
            var returnedPokemonNames = objectResult.Value as IEnumerable<string>;
            returnedPokemonNames.Count().Should().Be(count);
        }

        [Test]
        public async Task GivenPokemonNameProvided_WhenGetPokemons_ThenReturnPokemonTranslation()
        {
            // Arrange
            string origDescr = "original description";
            string translatedDescr = "translated description";
            var pokemon = new Pokemon
            {
                IsLegendary = true,
                Habitat = new PokemonHabitat
                {
                    Name = "cave"
                },
                FlovorTextEntries = new List<FlavorTextEntries>
                {
                    new FlavorTextEntries
                    {
                        FlavorText = origDescr,
                        Language = new Language{ Name = "en" }
                    }
                }
            };

            var fakePokemonService = A.Fake<PokemonService>();
            var fakeTranslationRepo = A.Fake<IDescriptionTranslationRepository>();
            A.CallTo(() => fakeTranslationRepo.TranslateAsync(origDescr, TranslationType.Yoda)).Returns(Task.FromResult(translatedDescr));

            // Act
            var pokedex = await fakePokemonService.Convert(pokemon, fakeTranslationRepo);

            // Assert
            pokedex.TranslationType.Should().Be(TranslationType.Yoda);
            pokedex.Description.Should().Be(translatedDescr);
        }

        [Test]
        public async Task GivenTranslationRequested_WhenTranslationLimitIsExceeded_ThenDoNotTranslate()
        {
            // Arrange
            string origDescr = "original description";
            var pokemon = new Pokemon
            {
                IsLegendary = true,
                Habitat = new PokemonHabitat
                {
                    Name = "cave"
                },
                FlovorTextEntries = new List<FlavorTextEntries>
                {
                    new FlavorTextEntries
                    {
                        FlavorText = origDescr,
                        Language = new Language{ Name = "en" }
                    }
                }
            };
            string httpResponseMessage = "{\"error\": {\"code\": 429, \"message\": \"Too Many Requests: Rate limit of 5 requests per hour exceeded. Please wait for 59 minutes and 51 seconds.\"} }";
            IConfiguration fakeConfiguration = new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string> {
                    {"Translations:FunTranslationsUrl", "https://fake.url"},
            }).Build();

            var fakePokemonService = A.Fake<PokemonService>();
            var fakeMessageHandler = FakeHttpMessageHandler.GetHttpMessageHandler(httpResponseMessage, HttpStatusCode.TooManyRequests);
            var fakeHttpClient = new HttpClient(fakeMessageHandler);

            IDescriptionTranslationRepository fakeTranslationRepo = new FunTranslationsRepository(fakeHttpClient, fakeConfiguration);

            // Act
            var pokedex = await fakePokemonService.Convert(pokemon, fakeTranslationRepo);

            // Assert
            pokedex.TranslationType.Should().Be(TranslationType.None);
            pokedex.Description.Should().Be(origDescr);
        }
    }

    class FakeHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage _response;

        public static HttpMessageHandler GetHttpMessageHandler(string content, HttpStatusCode httpStatusCode)
        {
            var memStream = new MemoryStream();

            var sw = new StreamWriter(memStream);
            sw.Write(content);
            sw.Flush();
            memStream.Position = 0;

            var httpContent = new StreamContent(memStream);

            var response = new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = httpContent
            };

            var messageHandler = new FakeHttpMessageHandler(response);

            return messageHandler;
        }

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            tcs.SetResult(_response);

            return tcs.Task;
        }
    }
}
