using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cropper.Blazor.Base
{
    public class CropperComponentBase : ICropperComponentBase
    {
        [JSInvokable(nameof(CropperComponentBase.IsReady))]
        public void IsReady(EventArgs eventArgs)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsStarted))]
        public void CropperIsStarted(EventArgs eventArgs)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsMoved))]
        public void CropperIsMoved(EventArgs eventArgs)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsEnded))]
        public void CropperIsEnded(EventArgs eventArgs)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsCroped))]
        public void CropperIsCroped(EventArgs eventArgs)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsZoomed))]
        public void CropperIsZoomed(EventArgs eventArgs)
        {

        }
    }
}
