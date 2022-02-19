using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Events
{
    public enum ActionEvent
    {
        Crop,
        Move,
        Zoom,
        E,
        S,
        W, 
        N,
        Ne, 
        Nw, 
        Se, 
        Sw,
        All
    }
}
