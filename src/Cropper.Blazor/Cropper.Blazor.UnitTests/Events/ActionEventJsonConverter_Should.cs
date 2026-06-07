using System.Text.Json;
using Cropper.Blazor.Events;
using Event = Cropper.Blazor.Events.CropStartEvent.CropStartEvent;
using Xunit;

namespace Cropper.Blazor.UnitTests.Events
{
    public class ActionEventJsonConverter_Should
    {
        [Theory]
        [InlineData("select", ActionEvent.Crop)]
        [InlineData("crop", ActionEvent.Crop)]
        [InlineData("move", ActionEvent.Move)]
        [InlineData("scale", ActionEvent.Zoom)]
        [InlineData("zoom", ActionEvent.Zoom)]
        [InlineData("e-resize", ActionEvent.E)]
        [InlineData("s-resize", ActionEvent.S)]
        [InlineData("w-resize", ActionEvent.W)]
        [InlineData("n-resize", ActionEvent.N)]
        [InlineData("ne-resize", ActionEvent.Ne)]
        [InlineData("nw-resize", ActionEvent.Nw)]
        [InlineData("se-resize", ActionEvent.Se)]
        [InlineData("sw-resize", ActionEvent.Sw)]
        [InlineData("transform", ActionEvent.All)]
        [InlineData("all", ActionEvent.All)]
        [InlineData("none", ActionEvent.None)]
        public void Deserialize_CropperJsActionValues(string action, ActionEvent expectedAction)
        {
            // arrange
            string json = "{\"action\":\"" + action + "\"}";

            // act
            Event? result = JsonSerializer.Deserialize<Event>(json);

            // assert
            Assert.Equal(expectedAction, result!.ActionEvent);
        }

        [Theory]
        [InlineData(ActionEvent.Crop, "select")]
        [InlineData(ActionEvent.Move, "move")]
        [InlineData(ActionEvent.Zoom, "scale")]
        [InlineData(ActionEvent.E, "e-resize")]
        [InlineData(ActionEvent.S, "s-resize")]
        [InlineData(ActionEvent.W, "w-resize")]
        [InlineData(ActionEvent.N, "n-resize")]
        [InlineData(ActionEvent.Ne, "ne-resize")]
        [InlineData(ActionEvent.Nw, "nw-resize")]
        [InlineData(ActionEvent.Se, "se-resize")]
        [InlineData(ActionEvent.Sw, "sw-resize")]
        [InlineData(ActionEvent.All, "transform")]
        [InlineData(ActionEvent.None, "none")]
        public void Serialize_CropperJsActionValues(ActionEvent action, string expectedAction)
        {
            // arrange
            var @event = new Event { ActionEvent = action };

            // act
            string json = JsonSerializer.Serialize(@event);

            // assert
            Assert.Contains($"\"action\":\"{expectedAction}\"", json);
        }
    }
}
