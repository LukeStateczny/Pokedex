using System;
using System.Threading.Tasks;

namespace DataRepository.Translations
{
    public interface IDescriptionTranslationRepository
    {
        Task<string> TranslateAsync(string description, TranslationType translationType);
    }
}
