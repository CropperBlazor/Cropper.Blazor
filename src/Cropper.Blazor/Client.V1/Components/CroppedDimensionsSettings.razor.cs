using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components
{
    public partial class CroppedDimensionsSettings
    {
        private decimal? minimumWidth = null;
        private decimal? maximumWidth = null;
        private decimal? minimumHeight = null;
        private decimal? maximumHeight = null;

        [CascadingParameter(Name = "ResetCropperAction"), Required]
        public Action ResetCropperAction { get; set; } = null!;

        public decimal? MinimumWidth { get => minimumWidth; set { minimumWidth = value; ResetCropperAction.Invoke(); } }
        public decimal? MaximumWidth { get => maximumWidth; set { maximumWidth = value; ResetCropperAction.Invoke(); } }
        public decimal? MinimumHeight { get => minimumHeight; set { minimumHeight = value; ResetCropperAction.Invoke(); } }
        public decimal? MaximumHeight { get => maximumHeight; set { maximumHeight = value; ResetCropperAction.Invoke(); } }
    }
}
