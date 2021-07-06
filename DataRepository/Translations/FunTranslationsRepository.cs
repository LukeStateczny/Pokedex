using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core.Parsers;
using Microsoft.Extensions.Configuration;

namespace DataRepository.Translations
{
    public class FunTranslationsRepository : IDescriptionTranslationRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _webRepositoryUrl;

        public FunTranslationsRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _webRepositoryUrl = _configuration.GetValue<string>("Translations:FunTranslationsUrl");
        }

        /// <summary>
        /// This method translates 'description' according to 'tranlationType' parameter.
        /// The 'Translations:FunTranslationsUrl' setting needs to be configured in appsettings.json file prior to use.
        /// </summary>
        /// <param name="description">Description to be translated</param>
        /// <param name="translationType">Translation style</param>
        /// <returns>Translated description or thrrows an error it franslation was not possible</returns>
        public async Task<string> TranslateAsync(string description, TranslationType translationType)
        {
            string translationService = translationType switch
            {
                TranslationType.Shakespeare => "translate/shakespeare.json",
                TranslationType.Yoda => "translate/yoda.json"
            };
            var translationText = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("text", description)
                };

            _httpClient.BaseAddress = new Uri(_webRepositoryUrl);
            var request = new HttpRequestMessage(HttpMethod.Post, translationService);
            request.Content = new FormUrlEncodedContent(translationText);

            using var response = await _httpClient.SendAsync(request);
            
            string content = await response.Content?.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                if (TranslationParsers.TryParseTranslation(content, out var funTranslation))
                {
                    return funTranslation.Contents?.Translated ?? description;
                }
            } else
            {
                throw new HttpRequestException(content);
            }

            return description;
        }
    }
}
