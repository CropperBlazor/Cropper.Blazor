using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cropper.Blazor.Models
{
    internal sealed class EnumMemberJsonConverter<TEnum> : JsonConverter<TEnum>
        where TEnum : struct, Enum
    {
        private static readonly IReadOnlyDictionary<TEnum, string> ValuesByEnum = CreateValuesByEnum();
        private static readonly IReadOnlyDictionary<string, TEnum> EnumsByValue = ValuesByEnum.ToDictionary(
            pair => pair.Value,
            pair => pair.Key,
            StringComparer.OrdinalIgnoreCase);

        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unable to convert token type {reader.TokenType} to {typeof(TEnum).Name}.");
            }

            string? value = reader.GetString();

            if (value is not null && EnumsByValue.TryGetValue(value, out TEnum enumValue))
            {
                return enumValue;
            }

            throw new JsonException($"Unable to convert value '{value}' to {typeof(TEnum).Name}.");
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            if (!ValuesByEnum.TryGetValue(value, out string? enumValue))
            {
                enumValue = value.ToString();
            }

            writer.WriteStringValue(enumValue);
        }

        private static IReadOnlyDictionary<TEnum, string> CreateValuesByEnum()
        {
            return Enum.GetValues<TEnum>()
                .ToDictionary(
                    value => value,
                    value => typeof(TEnum)
                        .GetField(value.ToString())?
                        .GetCustomAttribute<EnumMemberAttribute>()?
                        .Value ?? value.ToString());
        }
    }
}
