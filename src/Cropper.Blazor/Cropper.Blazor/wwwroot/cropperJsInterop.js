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
        const options = {
            aspectRatio: optionsImage.aspectRatio,
            preview: optionsImage.preview,
            ready: function (event) {
                imageObject.invokeMethodAsync('IsReady', event);
            },
            cropstart: function (event) {
                imageObject.invokeMethodAsync('CropperIsStarted', event);
            },
            cropmove: function (event) {
                imageObject.invokeMethodAsync('CropperIsMoved', event);
            },
            cropend: function (event) {
                imageObject.invokeMethodAsync('CropperIsEnded', event);
            },
            crop: function (event) {
                imageObject.invokeMethodAsync('CropperIsCroped', event);
            },
            zoom: function (e) {
                imageObject.invokeMethodAsync('CropperIsZoomed', event);
            }
        };
        this.cropperInstance = new Cropper(image, options);
    }
}

window.cropper = new CropperDecorator();