export class ToolsGetNextObjectIdRequest {
    path: string | undefined;
    objectType: string;
    
    constructor(newPath: string | undefined, newObjectType: string) {
        this.path = newPath;
        this.objectType = newObjectType;
    }
}