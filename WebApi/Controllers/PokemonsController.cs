using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Core.BusinessLogic;
using Core.Models;
using DataRepository.Pokemons;
using DataRepository.Translations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.ValidationAttributes;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    public class PokemonsController : ControllerBase
    {
        private readonly IPokemonDataRepository _pokemonRepository;
        private readonly IDescriptionTranslationRepository _translatorRepo;
        private readonly ILogger<PokemonsController> _logger;

        public PokemonsController(IPokemonDataRepository pokemonRepository, IDescriptionTranslationRepository translatorRepo, ILogger<PokemonsController> logger)
        {
            _pokemonRepository = pokemonRepository;
            _translatorRepo = translatorRepo;
            _logger = logger;
        }

        [HttpGet()]
        [MapToApiVersion("1.0")]
        [EnsurePositiveNumber]
        public async Task<ActionResult<IEnumerable<string>>> GetPokemonsV1([FromQuery(Name = "limit")] int limit = 20)
        {
            try
            {
                var pokemonService = new PokemonService(_pokemonRepository);
                var res = await pokemonService.List(limit);
                if (res == null)
                {
                    return NotFound();
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{idOrName}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Pokedex>> GetPokemonV1([FromRoute] string idOrName)
        {
            try
            {
                var pokemonService = new PokemonService(_pokemonRepository);
                var pokemon = await pokemonService.Read(idOrName);
                if (pokemon == null)
                {
                    return NotFound();
                }
                var pokedex = pokemonService.Convert(pokemon);
                return Ok(pokedex);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{idOrName}/translated")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<Pokedex>> GetTranslatedPokemonV1([FromRoute] string idOrName)
        {
            try
            {
                var pokemonService = new PokemonService(_pokemonRepository);
                var pokemon = await pokemonService.Read(idOrName);
                if (pokemon == null)
                {
                    return NotFound();
                }
                var pokedex = await pokemonService.Convert(pokemon, _translatorRepo);
                return Ok(pokedex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
