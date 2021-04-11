export class ToolsDocumentContentChangeRequest {
    path: string | undefined;
    content: string | undefined;
    returnSymbols: boolean;

    constructor(path: string | undefined, content: string | undefined, retSymbols: boolean) {
        this.path = path;
        this.content = content;
        this.returnSymbols = retSymbols;
    }
}