using System.Text.Json;
using System.Text.Json.Serialization;

namespace Asuse.Ai.Reports.Converters
{
    /// <summary>
    /// Custom JsonConverter for Dictionary&lt;string, object&gt; that converts JsonElement values
    /// to their proper .NET types during deserialization.
    /// </summary>
    public class ObjectDictionaryJsonConverter : JsonConverter<Dictionary<string, object>>
    {
        public override Dictionary<string, object> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected StartObject, found {reader.TokenType}");
            }

            var dictionary = new Dictionary<string, object>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Expected PropertyName, found {reader.TokenType}");
                }

                string propertyName = reader.GetString() ?? string.Empty;
                reader.Read();

                dictionary[propertyName] = ReadValue(ref reader, options);
            }

            throw new JsonException("Unexpected end of JSON");
        }

        public override void Write(
            Utf8JsonWriter writer,
            Dictionary<string, object> value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var kvp in value)
            {
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(kvp.Key) ?? kvp.Key);
                WriteValue(writer, kvp.Value, options);
            }

            writer.WriteEndObject();
        }

        private object ReadValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number => ReadNumber(ref reader),
                JsonTokenType.String => reader.GetString() ?? string.Empty,
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Null => null!,
                JsonTokenType.StartArray => ReadArray(ref reader, options),
                JsonTokenType.StartObject => ReadObject(ref reader, options),
                _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
            };
        }

        private object ReadNumber(ref Utf8JsonReader reader)
        {
            // Try to read as int first, fallback to decimal if it doesn't fit
            if (reader.TryGetInt32(out int intValue))
            {
                return intValue;
            }

            if (reader.TryGetInt64(out long longValue))
            {
                // If it fits in int range, return int for consistency
                if (longValue >= int.MinValue && longValue <= int.MaxValue)
                {
                    return (int)longValue;
                }
                return longValue;
            }

            // Use decimal for floating point numbers and large numbers
            if (reader.TryGetDecimal(out decimal decimalValue))
            {
                return decimalValue;
            }

            // Fallback: read as double
            return reader.GetDouble();
        }

        private List<object> ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var list = new List<object>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return list;
                }

                list.Add(ReadValue(ref reader, options));
            }

            throw new JsonException("Unexpected end of JSON array");
        }

        private Dictionary<string, object> ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            var dictionary = new Dictionary<string, object>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return dictionary;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException($"Expected PropertyName, found {reader.TokenType}");
                }

                string propertyName = reader.GetString() ?? string.Empty;
                reader.Read();

                dictionary[propertyName] = ReadValue(ref reader, options);
            }

            throw new JsonException("Unexpected end of JSON object");
        }

        private void WriteValue(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    writer.WriteNullValue();
                    break;
                case bool boolValue:
                    writer.WriteBooleanValue(boolValue);
                    break;
                case byte byteValue:
                    writer.WriteNumberValue(byteValue);
                    break;
                case sbyte sbyteValue:
                    writer.WriteNumberValue(sbyteValue);
                    break;
                case short shortValue:
                    writer.WriteNumberValue(shortValue);
                    break;
                case ushort ushortValue:
                    writer.WriteNumberValue(ushortValue);
                    break;
                case int intValue:
                    writer.WriteNumberValue(intValue);
                    break;
                case uint uintValue:
                    writer.WriteNumberValue(uintValue);
                    break;
                case long longValue:
                    writer.WriteNumberValue(longValue);
                    break;
                case ulong ulongValue:
                    writer.WriteNumberValue(ulongValue);
                    break;
                case float floatValue:
                    writer.WriteNumberValue(floatValue);
                    break;
                case double doubleValue:
                    writer.WriteNumberValue(doubleValue);
                    break;
                case decimal decimalValue:
                    writer.WriteNumberValue(decimalValue);
                    break;
                case string stringValue:
                    writer.WriteStringValue(stringValue);
                    break;
                case IEnumerable<object> enumerable:
                    writer.WriteStartArray();
                    foreach (var item in enumerable)
                    {
                        WriteValue(writer, item, options);
                    }
                    writer.WriteEndArray();
                    break;
                case Dictionary<string, object> dict:
                    Write(writer, dict, options);
                    break;
                default:
                    // For other types, use default serialization
                    JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    break;
            }
        }
    }
}
