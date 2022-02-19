class CropperDecorator {

    constructor() {
        this.cropperInstance = new Object();
    }

    initCropper(image, optionsImage, imageObject) {
        //if (image == null) {
        //    throw "Parameter 'image' must be is not null!";
        //}
        //if (optionsImage == null) {
        //    throw "Parameter 'optionsImage' must be is not null!";
        //}
        //if (imageObject == null) {
        //    throw "Parameter 'imageObject' must be is not null!";
        //}

        //TO DO add dynamic fields to this object///
        const options = {
            //dragMode: optionsImage.dragMode,
            viewMode: optionsImage.viewMode,
            aspectRatio: optionsImage.aspectRatio,
            preview: optionsImage.preview,
            ready: function (event) {
                imageObject.invokeMethodAsync('IsReady', event);
            },
            cropstart: function (event) {
                imageObject.invokeMethodAsync('CropperIsStarted', event.detail);
            },
            cropmove: function (event) {
                imageObject.invokeMethodAsync('CropperIsMoved', event.detail);
            },
            cropend: function (event) {
                imageObject.invokeMethodAsync('CropperIsEnded', event.detail);
            },
            crop: function (event) {
                imageObject.invokeMethodAsync('CropperIsCroped', event.detail);
            },
            zoom: function (event) {
                imageObject.invokeMethodAsync('CropperIsZoomed', event.detail);
            }
        };
        console.log(options);
        this.cropperInstance = new Cropper(image, options);
    }
}

window.cropper = new CropperDecorator();