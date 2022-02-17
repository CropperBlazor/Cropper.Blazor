using Cropper.Blazor.Base;
using Cropper.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Services
{
    public interface ICropperJsInterop
    {
        public Task LoadAsync();
        public ValueTask InitCropper([NotNull] ElementReference image, [NotNull] Options options, [NotNull] DotNetObjectReference<ICropperComponentBase> cropperComponentBase);
    }
}
