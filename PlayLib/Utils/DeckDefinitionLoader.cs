using System.Text.Json;

using CrazyEights.PlayLib.Data;

namespace CrazyEights.PlayLib.Utils
{
    public static class DeckDefinitionLoader
    {
        public static T Load<T>(string jsonString) where T : DeckDefinition
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var cards = JsonSerializer.Deserialize<T>(jsonString, options);
            return cards;
        }
    }
}