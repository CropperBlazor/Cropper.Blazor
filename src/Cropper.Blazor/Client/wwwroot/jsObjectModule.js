class JsObject {
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
}

window.jsObject = new JsObject();
