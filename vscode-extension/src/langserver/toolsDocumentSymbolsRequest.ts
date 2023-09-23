export class ToolsDocumentSymbolsRequest {
    source : string;
    path : string;
    projectPath: string | undefined;
    includeProperties : boolean;
    isActiveDocument: boolean;

    constructor(newSourceCode : string, newPath: string, newProjectPath: string | undefined, newIncludeProperties: boolean, newIsActiveDocument: boolean) {
        this.source = newSourceCode;
        this.path = newPath;
        this.projectPath = newProjectPath;
        this.includeProperties = newIncludeProperties;
        this.isActiveDocument = newIsActiveDocument;
    }

}