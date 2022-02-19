using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Models
{
    public enum DragMode
    {
        [EnumMember(Value = "crop")]
        Crop,
        [EnumMember(Value = "move")]
        Move,
        [EnumMember(Value = "none")]
        None
    }
}
