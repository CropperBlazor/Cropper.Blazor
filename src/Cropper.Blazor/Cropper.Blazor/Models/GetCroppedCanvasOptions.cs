﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Models
{
    public class GetCroppedCanvasOptions
    {
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? MinWidth { get; set; }
        public decimal? MinHeight { get; set; }
        public decimal? MaxWidth { get; set; }
        public decimal? MaxHeight { get; set; }
        public string? FillColor { get; set; }
        public bool? ImageSmoothingEnabled { get; set; }
        [EnumDataType(typeof(ImageSmoothingQuality))]
        public string? ImageSmoothingQuality { get; set; }
    }
}