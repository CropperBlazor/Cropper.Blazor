using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Models
{
    public class CropperData
    {
        [JsonProperty("x", NullValueHandling = NullValueHandling.Ignore)]
        public decimal X { get; set; }

        [JsonProperty("y", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Y { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Height { get; set; }

        [JsonProperty("rotate", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Rotate { get; set; }

        [JsonProperty("scaleX", NullValueHandling = NullValueHandling.Ignore)]
        public decimal ScaleX { get; set; }

        [JsonProperty("scaleY", NullValueHandling = NullValueHandling.Ignore)]
        public decimal ScaleY { get; set; }
    }
}
