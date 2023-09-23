export class ToolsGetSyntaxTreeRequest {
    source : string;
    path : string;
    projectPath: string | undefined;
    open: boolean;

    constructor(newSourceCode : string, newPath: string, newProjectPath: string | undefined, newOpen: boolean) {
        this.source = newSourceCode;
        this.path = newPath;
        this.projectPath = newProjectPath;
        this.open = newOpen;
    }

}