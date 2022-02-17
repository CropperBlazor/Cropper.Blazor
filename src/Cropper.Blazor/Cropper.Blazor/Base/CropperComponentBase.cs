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
        public void IsReady(object data)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsStarted))]
        public void CropperIsStarted(object data)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsMoved))]
        public void CropperIsMoved(object data)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsEnded))]
        public void CropperIsEnded(object data)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsCroped))]
        public void CropperIsCroped(object data)
        {

        }

        [JSInvokable(nameof(CropperComponentBase.CropperIsZoomed))]
        public void CropperIsZoomed(object data)
        {

        }
    }
}
