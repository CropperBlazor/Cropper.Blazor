class JsObject {
    getPropertyList(path: string): string[] {
        let res: string[] = path.replace('[', '.').replace(']', '').split('.');

        if (res[0] === "") {
            res.shift();
        }

        return res;
    }

    getInstanceProperty(instance: any, propertyPath: string): any {
        if (propertyPath === '') {
            return instance;
        }

        let currentProperty: any = instance;
        let splitProperty: string[] = this.getPropertyList(propertyPath);

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

(window as any).jsObject = new JsObject();