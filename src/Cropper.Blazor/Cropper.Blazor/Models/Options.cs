using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Models
{
    public class Options
    {
        [JsonProperty("aspectRatio", NullValueHandling = NullValueHandling.Ignore)]
        public float? AspectRatio { get; set; }

        [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
        public string Preview { get; set; }
    }
}
