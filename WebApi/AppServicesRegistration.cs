using System;
using DataRepository;
using DataRepository.Pokemons;
using DataRepository.Translations;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public static class AppServicesRegistration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddHttpClient<IPokemonDataRepository, PokemonDataRepository>();
            services.AddHttpClient<IDescriptionTranslationRepository, FunTranslationsRepository>();
        }
    }
}
