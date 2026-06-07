using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cropper.Blazor.Events
{
    internal sealed class ActionEventJsonConverter : JsonConverter<ActionEvent>
    {
        public override ActionEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unable to convert token type {reader.TokenType} to {nameof(ActionEvent)}.");
            }

            string? value = reader.GetString();

            return value switch
            {
                "select" or "crop" => ActionEvent.Crop,
                "move" => ActionEvent.Move,
                "scale" or "zoom" => ActionEvent.Zoom,
                "e-resize" or "e" => ActionEvent.E,
                "s-resize" or "s" => ActionEvent.S,
                "w-resize" or "w" => ActionEvent.W,
                "n-resize" or "n" => ActionEvent.N,
                "ne-resize" or "ne" => ActionEvent.Ne,
                "nw-resize" or "nw" => ActionEvent.Nw,
                "se-resize" or "se" => ActionEvent.Se,
                "sw-resize" or "sw" => ActionEvent.Sw,
                "transform" or "all" => ActionEvent.All,
                "none" or null or "" => ActionEvent.None,
                _ when Enum.TryParse(value, ignoreCase: true, out ActionEvent actionEvent) => actionEvent,
                _ => throw new JsonException($"Unable to convert value '{value}' to {nameof(ActionEvent)}."),
            };
        }

        public override void Write(Utf8JsonWriter writer, ActionEvent value, JsonSerializerOptions options)
        {
            string action = value switch
            {
                ActionEvent.Crop => "select",
                ActionEvent.Move => "move",
                ActionEvent.Zoom => "scale",
                ActionEvent.E => "e-resize",
                ActionEvent.S => "s-resize",
                ActionEvent.W => "w-resize",
                ActionEvent.N => "n-resize",
                ActionEvent.Ne => "ne-resize",
                ActionEvent.Nw => "nw-resize",
                ActionEvent.Se => "se-resize",
                ActionEvent.Sw => "sw-resize",
                ActionEvent.All => "transform",
                ActionEvent.None => "none",
                _ => "none",
            };

            writer.WriteStringValue(action);
        }
    }
}
