using System;
using Newtonsoft.Json;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Enumeration for database object types.
    /// String values are preserved to maintain consistency with existing database records.
    /// </summary>
    [JsonConverter(typeof(DbObjectTypeJsonConverter))]
    public enum DbObjectType
    {
        Table,
        View,
        TableOrView,
        MatView,
        Procedure,
        TempTable
    }

    /// <summary>
    /// JSON converter for DbObjectType that serializes/deserializes using database string values.
    /// </summary>
    public class DbObjectTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DbObjectType) || objectType == typeof(DbObjectType?);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();
            else
                writer.WriteValue(((DbObjectType)value).ToDatabaseString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return objectType == typeof(DbObjectType?) ? null : (object)DbObjectType.Table;

            if (reader.TokenType == JsonToken.String)
            {
                var result = DbObjectTypeExtensions.FromDatabaseString(reader.Value?.ToString());
                return objectType == typeof(DbObjectType?) ? result : (result ?? DbObjectType.Table);
            }

            return objectType == typeof(DbObjectType?) ? null : (object)DbObjectType.Table;
        }
    }

    /// <summary>
    /// Extension methods for DbObjectType enum to handle string conversion for database storage.
    /// </summary>
    public static class DbObjectTypeExtensions
    {
        /// <summary>
        /// Converts the enum value to its database string representation.
        /// </summary>
        /// <param name="type">The enum value to convert</param>
        /// <returns>The database string value</returns>
        public static string ToDatabaseString(this DbObjectType type)
        {
            switch (type)
            {
                case DbObjectType.Table:
                    return "table";
                case DbObjectType.View:
                    return "view";
                case DbObjectType.TableOrView:
                    return "table or view";
                case DbObjectType.MatView:
                    return "mat view";
                case DbObjectType.Procedure:
                    return "procedure";
                case DbObjectType.TempTable:
                    return "temp table";
                default:
                    throw new ArgumentException($"Unknown DbObjectType: {type}", nameof(type));
            }
        }

        /// <summary>
        /// Parses a database string value to the corresponding enum value.
        /// </summary>
        /// <param name="value">The database string value</param>
        /// <returns>The enum value, or null if the string is null/empty or doesn't match any known value</returns>
        public static DbObjectType? FromDatabaseString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            switch (value)
            {
                case "table":
                    return DbObjectType.Table;
                case "view":
                    return DbObjectType.View;
                case "table or view":
                    return DbObjectType.TableOrView;
                case "mat view":
                    return DbObjectType.MatView;
                case "procedure":
                    return DbObjectType.Procedure;
                case "temp table":
                    return DbObjectType.TempTable;
                default:
                    return null;
            }
        }
    }
}
