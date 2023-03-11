export class ToolsGetFullSyntaxTreeRequest {
    source: string | undefined;
    path: string | undefined;

    constructor(newSource: string, newPath: string) {
        this.source = newSource;
        this.path = newPath;
    }

}