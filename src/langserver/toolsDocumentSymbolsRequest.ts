export class ToolsDocumentSymbolsRequest {
    source : string;
    path : string;
    includeProperties : boolean;

    constructor(newSourceCode : string, newPath: string, newIncludeProperties: boolean) {
        this.source = newSourceCode;
        this.path = newPath;
        this.includeProperties = newIncludeProperties;
    }

}