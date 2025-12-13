declare const DotNet: DotNetNamespace;

interface DotNetNamespace {
    invokeMethodAsync<T>(assemblyName: string, methodIdentifier: string, ...args: any[]): Promise<T>;

    createJSObjectReference(jsObject: any): JsObjectReference;
}

interface DotNetStreamReference {
    arrayBuffer(): Promise<ArrayBuffer>;
}

interface DotNetObjectReference<T> {
    invokeMethodAsync(methodName: keyof T, ...args: any[]): Promise<any>;
}

interface JsObjectReference {
    __jsObjectId: number;
}
