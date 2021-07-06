using System;
using System.Text.Json.Serialization;

namespace Core.Models
{
    public class Translation
    {
        [JsonPropertyName("contents")]
        public Contents Contents { get; set; }
        [JsonPropertyName("success")]
        public Success Success { get; set; }
    }

    public class Success
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }

    public class Contents
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
        [JsonPropertyName("translation")]
        public string Translation { get; set; }
    }
}
