export class ToolsGetSyntaxTreeRequest {
    source : string;
    path : string;
    open: boolean;

    constructor(newSourceCode : string, newPath: string, newOpen: boolean) {
        this.source = newSourceCode;
        this.path = newPath;
        this.open = newOpen;
    }

}