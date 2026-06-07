using System.ComponentModel.DataAnnotations;
using Cropper.Blazor.Components;
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
                InvokeAsync(ApplyAspectRatioRulesForCropperAsync);
            }
        }

        public decimal? MinAspectRatio
        {
            get => minAspectRatio;
            set
            {
                minAspectRatio = value;
                InvokeAsync(ApplyAspectRatioRulesForCropperAsync);
            }
        }

        [CascadingParameter(Name = "AspectRatio"), Required]
        private decimal? AspectRatio { get; set; }

        [CascadingParameter(Name = "CropperComponent"), Required]
        private CropperComponent CropperComponent { get; set; } = null!;

        [CascadingParameter(Name = "IsFreeAspectRatioEnabled"), Required]
        private bool IsFreeAspectRatioEnabled
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
                ContainerData containerData = await CropperComponent!.GetContainerDataAsync();
                CropBoxData cropBoxData = await CropperComponent!.GetCropBoxDataAsync();

                if (cropBoxData.Height != 0)
                {
                    decimal aspectRatio = cropBoxData.Width / cropBoxData.Height;

                    if (aspectRatio < minAspectRatio || aspectRatio > maxAspectRatio)
                    {
                        decimal? newCropBoxWidth = cropBoxData.Height * ((minAspectRatio + maxAspectRatio) / 2);
                        decimal? left = (containerData.Width - newCropBoxWidth) / 2;

                        CropperComponent!.SetCropBoxData(new SetCropBoxDataOptions
                        {
                            Left = left,
                            Width = newCropBoxWidth,
                        });

                        cropBoxData = await CropperComponent!.GetCropBoxDataAsync();
                        aspectRatio = cropBoxData.Width / cropBoxData.Height;
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
