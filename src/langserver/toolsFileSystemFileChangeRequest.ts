export class ToolsFileSystemFileChangeRequest {
    path: string | undefined;

    constructor(path: string | undefined) {
        this.path = path;
    }
}