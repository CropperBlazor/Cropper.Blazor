using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Models
{
    public class Options
    {
        [JsonProperty("aspectRatio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal AspectRatio { get; set; }

        [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
        public string Preview { get; set; }

        [JsonProperty("autoCrop", NullValueHandling = NullValueHandling.Ignore)]
        public bool AutoCrop { get; set; }

        [JsonProperty("autoCropArea", NullValueHandling = NullValueHandling.Ignore)]
        public decimal AutoCropArea { get; set; }

        [JsonProperty("background", NullValueHandling = NullValueHandling.Ignore)]
        public bool Background { get; set; }

        [JsonProperty("center", NullValueHandling = NullValueHandling.Ignore)]
        public bool Center { get; set; }

        [JsonProperty("checkCrossOrigin", NullValueHandling = NullValueHandling.Ignore)]
        public bool CheckCrossOrigin { get; set; }

        [JsonProperty("checkOrientation", NullValueHandling = NullValueHandling.Ignore)]
        public bool CheckOrientation { get; set; }

        [JsonProperty("cropBoxMovable", NullValueHandling = NullValueHandling.Ignore)]
        public bool CropBoxMovable { get; set; }

        [JsonProperty("cropBoxResizable", NullValueHandling = NullValueHandling.Ignore)]
        public bool CropBoxResizable { get; set; }

        [JsonProperty("setDataOptions", NullValueHandling = NullValueHandling.Ignore)]
        public SetDataOptions SetDataOptions { get; set; }

        [JsonProperty("dragMode", NullValueHandling = NullValueHandling.Ignore)]
        [EnumDataType(typeof(DragMode))]
        public string DragMode { get; set; }

        [JsonProperty("guides", NullValueHandling = NullValueHandling.Ignore)]
        public bool Guides { get; set; }

        [JsonProperty("highlight", NullValueHandling = NullValueHandling.Ignore)]
        public bool Highlight { get; set; }

        [JsonProperty("initialAspectRatio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal InitialAspectRatio { get; set; }

        [JsonProperty("minCanvasHeight", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinCanvasHeight { get; set; }

        [JsonProperty("minCanvasWidth", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinCanvasWidth { get; set; }

        [JsonProperty("minContainerHeight", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinContainerHeight { get; set; }

        [JsonProperty("minContainerWidth", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinContainerWidth { get; set; }

        [JsonProperty("minCropBoxHeight", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinCropBoxHeight { get; set; }

        [JsonProperty("minCropBoxWidth", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinCropBoxWidth { get; set; }

        [JsonProperty("modal", NullValueHandling = NullValueHandling.Ignore)]
        public bool Modal { get; set; }

        [JsonProperty("movable", NullValueHandling = NullValueHandling.Ignore)]
        public bool Movable { get; set; }

        [JsonProperty("responsive", NullValueHandling = NullValueHandling.Ignore)]
        public bool Responsive { get; set; }

        [JsonProperty("restore", NullValueHandling = NullValueHandling.Ignore)]
        public bool Restore { get; set; }

        [JsonProperty("rotatable", NullValueHandling = NullValueHandling.Ignore)]
        public bool Rotatable { get; set; }

        [JsonProperty("scalable", NullValueHandling = NullValueHandling.Ignore)]
        public bool Scalable { get; set; }

        [JsonProperty("toggleDragModeOnDblclick", NullValueHandling = NullValueHandling.Ignore)]
        public bool ToggleDragModeOnDblclick { get; set; }

        [JsonProperty("viewMode", NullValueHandling = NullValueHandling.Ignore)]
        public ViewMode ViewMode { get; set; }

        [JsonProperty("wheelZoomRatio", NullValueHandling = NullValueHandling.Ignore)]
        public decimal WheelZoomRatio { get; set; }

        [JsonProperty("zoomOnTouch", NullValueHandling = NullValueHandling.Ignore)]
        public bool ZoomOnTouch { get; set; }

        [JsonProperty("zoomOnWheel", NullValueHandling = NullValueHandling.Ignore)]
        public bool ZoomOnWheel { get; set; }

        [JsonProperty("zoomable", NullValueHandling = NullValueHandling.Ignore)]
        public bool Zoomable { get; set; }
    }
}
