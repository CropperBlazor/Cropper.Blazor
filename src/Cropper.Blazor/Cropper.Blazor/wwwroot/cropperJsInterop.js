class CropperDecorator {

    constructor() {
        this.cropperInstance = new Object();
    }

    initCropper(image, optionsImage, imageObject) {
        if (image == null) {
            throw "Parameter 'image' must be is not null!";
        }
        const options = {};
        if (imageObject != null) {
            options['ready'] = function (event) {
                imageObject.invokeMethodAsync('IsReady', event);
            };
            options['cropstart'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsStarted', event.detail);
            };
            options['cropmove'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsMoved', event.detail);
            };
            options['cropend'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsEnded', event.detail);
            };
            options['crop'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsCroped', event.detail);
            };
            options['zoom'] = function (event) {
                imageObject.invokeMethodAsync('CropperIsZoomed', event.detail);
            };
        }
        if (optionsImage != null) {
            Object.entries(optionsImage)?.forEach(([key, value]) => {
                options[key] = value;
            });
        }
        this.cropperInstance = new Cropper(image, options);
    }
}

window.cropper = new CropperDecorator();