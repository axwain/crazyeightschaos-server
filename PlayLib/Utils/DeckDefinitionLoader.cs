using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

using CrazyEights.PlayLib.Data;
using CrazyEights.PlayLib.Enums;

namespace CrazyEights.PlayLib.Utils
{
    public class CardDataJsonConverter : JsonConverter<CardData>
    {
        public override CardData Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var validProperties = new HashSet<string>();
            validProperties.Add("effectId");
            validProperties.Add("effectArgs");
            validProperties.Add("maxBaseCopies");
            validProperties.Add("maxExtendedCopies");
            var values = new List<JsonElement>(4);
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var properties = jsonDoc.RootElement.EnumerateObject().ToArray();

                if (properties.Length != 4)
                {
                    throw new InvalidOperationException(
                        "Error deserializing: Card Data has invalid number of properties"
                    );
                }

                foreach (var property in properties)
                {
                    if (validProperties.Contains(property.Name))
                    {
                        validProperties.Remove(property.Name);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Error deserializing: {property} is not a valid Card Data property"
                        );
                    }
                }

                return new CardData(
                    (Effects)jsonDoc.RootElement.GetProperty("effectId").GetInt32(),
                    jsonDoc.RootElement.GetProperty("effectArgs").GetInt32(),
                    jsonDoc.RootElement.GetProperty("maxBaseCopies").GetInt32(),
                    jsonDoc.RootElement.GetProperty("maxExtendedCopies").GetInt32()
                );
            }

            throw new InvalidOperationException("Can't deserialize card data");
        }

        public override void Write(Utf8JsonWriter writer, CardData value, JsonSerializerOptions options)
        {
            throw new InvalidOperationException();
        }
    }

    public static class DeckDefinitionLoader
    {
        public static T Load<T>(string jsonString) where T : DeckDefinition
        {
            var options = new JsonSerializerOptions
            {
                MaxDepth = 3,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            options.Converters.Add(new CardDataJsonConverter());

            var cards = JsonSerializer.Deserialize<T>(jsonString, options);
            return cards;
        }
    }
}