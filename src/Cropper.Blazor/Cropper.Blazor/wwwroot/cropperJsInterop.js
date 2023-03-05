class CropperDecorator {

    constructor() {
        this.cropperInstance = {};
    }

    clear() {
        return this.cropperInstance.clear();
    }

    crop() {
        return this.cropperInstance.crop();
    }

    destroy() {
        return this.cropperInstance.destroy();
    }

    disable() {
        return this.cropperInstance.disable();
    }

    enable() {
        return this.cropperInstance.enable();
    }

    getCanvasData() {
        return this.cropperInstance.getCanvasData();
    }

    getContainerData() {
        return this.cropperInstance.getContainerData();
    }

    getCropBoxData() {
        return this.cropperInstance.getCropBoxData();
    }

    getCroppedCanvas(options) {
        var c = this.cropperInstance.getCroppedCanvas(options);

        //console.log(c)
        //var ref = DotNet.createJSObjectReference(c);

        return c;
    }

    getCroppedCanvasDataURL(options) {
        return this.cropperInstance.getCroppedCanvas(options).toDataURL();
    }

    getData(rounded) {
        return this.cropperInstance.getData(rounded);
    }

    getImageData() {
        return this.cropperInstance.getImageData();
    }

    move(offsetX, offsetY) {
        return this.cropperInstance.move(offsetX, offsetY);
    }

    moveTo(x, y) {
        return this.cropperInstance.moveTo(x, y);
    }

    replace(url, onlyColorChanged) {
        return this.cropperInstance.replace(url, onlyColorChanged);
    }

    reset() {
        return this.cropperInstance.reset();
    }

    rotate(degree) {
        return this.cropperInstance.rotate(degree);
    }

    rotateTo(degree) {
        return this.cropperInstance.rotateTo(degree);
    }

    scale(scaleX, scaleY) {
        return this.cropperInstance.scale(scaleX, scaleY);
    }

    scaleX(scaleX) {
        return this.cropperInstance.scaleX(scaleX);
    }

    scaleY(scaleY) {
        return this.cropperInstance.scaleY(scaleY);
    }

    setAspectRatio(aspectRatio) {
        return this.cropperInstance.setAspectRatio(aspectRatio);
    }

    setCanvasData(data) {
        return this.cropperInstance.setCanvasData(data);
    }

    setCropBoxData(data) {
        return this.cropperInstance.setCropBoxData(data);
    }

    setData(data) {
        return this.cropperInstance.setData(data);
    }

    setDragMode(dragMode) {
        return this.cropperInstance.setDragMode(dragMode);
    }

    zoom(ratio) {
        return this.cropperInstance.zoom(ratio);
    }

    zoomTo(ratio, pivotX, pivotY) {
        return this.cropperInstance.zoomTo(ratio, {pivotX, pivotY});
    }

    noConflict() {
        return Cropper.noConflict();
    }

    setDefaults(options) {
        return Cropper.setDefaults(options);
    }

    async getImageUsingStreaming(imageStream) {
        const arrayBuffer = await imageStream.arrayBuffer();
        const blob = new Blob([arrayBuffer]);
        return URL.createObjectURL(blob);
    }

    revokeObjectUrl(url) {
        URL.revokeObjectURL(url);
    }

    initCropper(image, optionsImage, imageObject) {
        if (image == null) {
            throw "Parameter 'image' must be is not null!";
        }
        const options = {};
        if (imageObject != null) {
            options['ready'] = function (event) {
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('IsReady', eref);
                DotNet.disposeJSObjectReference(eref);
            };
            options['cropstart'] = function (event) {
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsStarted', eref);
                DotNet.disposeJSObjectReference(eref);
            };
            options['cropmove'] = function (event) {
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsMoved', eref);
                DotNet.disposeJSObjectReference(eref);
            };
            options['cropend'] = function (event) {
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsEnded', eref);
                DotNet.disposeJSObjectReference(eref);
            };
            options['crop'] = function (event) {
                console.log('Event log: ');
                console.log(event);
                console.log(event.detail);
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsCroped', eref);
                DotNet.disposeJSObjectReference(eref);
            };
            options['zoom'] = function (event) {
                let eref = DotNet.createJSObjectReference(event);
                imageObject.invokeMethodAsync('CropperIsZoomed', eref);
                DotNet.disposeJSObjectReference(eref);
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

class JsObject {

    getObjectProperty(obj, property) {
        console.log("Object: " + obj);
        return obj[property];
    }

    jSReference(element) {
        return element.valueOf();
    }

    setObjectProperty(obj, property, value) {
        obj[property] = value;
    }

    getPropertyList(path) {
        let res = path.replace('[', '.').replace(']', '').split('.');

        if (res[0] === "") { // if we pass "[0].id" we want to return [0,'id']
            res.shift();
        }

        return res;
    }

    getInstanceProperty(instance, propertyPath) {

        if (propertyPath === '') {
            return instance;
        }

        let currentProperty = instance;
        let splitProperty = this.getPropertyList(propertyPath);

        for (let i = 0; i < splitProperty.length; i++) {
            if (splitProperty[i] in currentProperty) {
                currentProperty = currentProperty[splitProperty[i]];
            } else {
                return null;
            }
        }

        return currentProperty;
    }

    callInstanceMethod (instance, methodPath, ...args) {
        if (methodPath.indexOf('.') >= 0) {
            //if it's a method call on a child object we get this child object so the method call will happen in the context of the child object
            //some method like window.locaStorage.setItem  will throw an exception if the context is not expected
            let instancePath = methodPath.substring(0, methodPath.lastIndexOf('.'));
            instance = this.getInstanceProperty(instance, instancePath);
            methodPath = methodPath.substring(methodPath.lastIndexOf('.') + 1);
        }

        for (let index = 0; index < args.length; index++) {
            const element = args[index];
            //we change null value to undefined as there is no way to pass undefined value from C# and most of the browser API use undefined instead of null value for "no value"
            if (element === null) {
                args[index] = undefined;
            }
        }

        let method = this.getInstanceProperty(instance, methodPath);

        return method.apply(instance, args);
    }

    returnInstance (instance, serializationSpec) {
        return this.getSerializableObject(instance, [], serializationSpec);
    }

    getSerializableObject (data, alreadySerialized, serializationSpec) {
    if (serializationSpec === false) {
        return undefined;
    }
    if (!alreadySerialized) {
        alreadySerialized = [];
    }
    if (typeof data == "undefined" ||
        data === null) {
        return null;
    }
    if (typeof data === "number" ||
        typeof data === "string" ||
        typeof data == "boolean") {
        return data;
    }
    let res = (Array.isArray(data)) ? [] : {};
    if (!serializationSpec) {
        serializationSpec = "*";
    }
    for (let i in data) {
        let currentMember = data[i];

        if (typeof currentMember === 'function' || currentMember === null) {
            continue;
        }
        let currentMemberSpec;
        if (serializationSpec != "*") {
            currentMemberSpec = Array.isArray(data) ? serializationSpec : serializationSpec[i];
            if (!currentMemberSpec) {
                continue;
            }
        } else {
            currentMemberSpec = "*"
        }
        if (typeof currentMember === 'object') {
            if (alreadySerialized.indexOf(currentMember) >= 0) {
                continue;
            }
            alreadySerialized.push(currentMember);
            if (Array.isArray(currentMember) || currentMember.length) {
                res[i] = [];
                for (let j = 0; j < currentMember.length; j++) {
                    const arrayItem = currentMember[j];
                    if (typeof arrayItem === 'object') {
                        res[i].push(this.getSerializableObject(arrayItem, alreadySerialized, currentMemberSpec));
                    } else {
                        res[i].push(arrayItem);
                    }
                }
            } else {
                //the browser provides some member (like plugins) as hash with index as key, if length == 0 we shall not convert it
                if (currentMember.length === 0) {
                    res[i] = [];
                } else {
                    res[i] = this.getSerializableObject(currentMember, alreadySerialized, currentMemberSpec);
                }
            }


        } else {
            // string, number or boolean
            if (currentMember === Infinity) { //inifity is not serialized by JSON.stringify
                currentMember = "Infinity";
            }
            if (currentMember !== null) { //needed because the default json serializer in jsinterop serialize null values
                res[i] = currentMember;
            }
        }
    }
    return res;
};
}

window.cropper = new CropperDecorator();
window.jsObject = new JsObject();