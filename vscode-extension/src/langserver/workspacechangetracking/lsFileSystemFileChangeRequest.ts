export class LSFileSystemFileChangeRequest {
    path: string | undefined;

    constructor(path: string | undefined) {
        this.path = path;
    }
}