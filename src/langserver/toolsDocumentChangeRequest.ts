export class ToolsDocumentChangeRequest {
    path: string | undefined;
    content: string | undefined;

    constructor(path: string | undefined, content: string | undefined) {
        this.path = path;
        this.content = content;
    }
}