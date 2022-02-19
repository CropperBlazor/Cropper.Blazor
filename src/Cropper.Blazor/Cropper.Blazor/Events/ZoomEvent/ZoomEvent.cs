using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cropper.Blazor.Events.ZoomEvent
{
    public class ZoomEvent
    {
        [JsonPropertyName("oldRatio")]
        public decimal OldRatio { get; set; }

        [JsonPropertyName("ratio")]
        public decimal Ratio { get; set; }
    }
}
