export class ToolsDocumentSymbolsRequest {
    source : string;
    path : string;

    constructor(newSourceCode : string, newPath: string) {
        this.source = newSourceCode;
        this.path = newPath;
    }

}