export class ToolsDocumentSymbolsRequest {
    source : string;
    path : string;
    includeProperties : boolean;
    isActiveDocument: boolean;

    constructor(newSourceCode : string, newPath: string, newIncludeProperties: boolean, newIsActiveDocument: boolean) {
        this.source = newSourceCode;
        this.path = newPath;
        this.includeProperties = newIncludeProperties;
        this.isActiveDocument = newIsActiveDocument;
    }

}