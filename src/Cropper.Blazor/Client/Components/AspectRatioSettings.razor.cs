﻿using System.ComponentModel.DataAnnotations;
using Cropper.Blazor.Client.Pages;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Components
{
    public partial class AspectRatioSettings
    {
        private decimal? maxAspectRatio;
        private decimal? minAspectRatio;
        private bool isEnableAspectRatioSettings;

        public decimal? MaxAspectRatio
        {
            get => maxAspectRatio;
            set
            {
                maxAspectRatio = value;
                ApplyAspectRatioRulesForCropperAsync();
            }
        }

        public decimal? MinAspectRatio
        {
            get => minAspectRatio;
            set
            {
                minAspectRatio = value;
                ApplyAspectRatioRulesForCropperAsync();
            }
        }

        [CascadingParameter(Name = "AspectRatio"), Required]
        private decimal? AspectRatio { get; set; }

        [CascadingParameter(Name = "CropperDemo"), Required]
        private CropperDemo CropperDemo { get; set; } = null!;

        [CascadingParameter(Name = "IsEnableAspectRatioSettings"), Required]
        private bool IsEnableAspectRatioSettings
        {
            get => isEnableAspectRatioSettings;
            set
            {
                if (!value)
                {
                    minAspectRatio = null;
                    maxAspectRatio = null;
                }

                isEnableAspectRatioSettings = value;
            }
        }

        public async Task ApplyAspectRatioRulesForCropperAsync()
        {
            if (minAspectRatio is not null || maxAspectRatio is not null)
            {
                ContainerData containerData = await CropperDemo.CropperComponent!.GetContainerDataAsync();
                CropBoxData cropBoxData = await CropperDemo.CropperComponent!.GetCropBoxDataAsync();

                if (cropBoxData.Height != 0)
                {
                    decimal aspectRatio = cropBoxData.Width / cropBoxData.Height;

                    if (aspectRatio < minAspectRatio || aspectRatio > maxAspectRatio)
                    {
                        decimal? newCropBoxWidth = cropBoxData.Height * ((minAspectRatio + maxAspectRatio) / 2);

                        CropperDemo.CropperComponent!.SetCropBoxData(new SetCropBoxDataOptions
                        {
                            Left = (containerData.Width - newCropBoxWidth) / 2,
                            Width = newCropBoxWidth,
                        });
                    }

                    SetUpAspectRatio(aspectRatio);
                }
            }
        }

        public void SetUpAspectRatio(decimal? aspectRatio)
        {
            AspectRatio = aspectRatio;
            StateHasChanged();
        }
    }
}