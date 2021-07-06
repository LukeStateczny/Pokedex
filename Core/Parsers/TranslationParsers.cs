using System;
using System.Text.Json;
using Core.Models;

namespace Core.Parsers
{
    public class TranslationParsers
    {
        static JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        public static bool TryParseTranslation(string json, out Translation t)
        {
            t = JsonSerializer.Deserialize<Translation>(json, serializeOptions);
            return t != null;
        }
    }
}
