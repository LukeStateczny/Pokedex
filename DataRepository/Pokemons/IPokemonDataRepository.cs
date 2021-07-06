using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace DataRepository.Pokemons
{
    public interface IPokemonDataRepository
    {
        Task<IEnumerable<string>> ReadAsync(int count);
        Task<Pokemon> ReadAsync(string name);
    }
}
