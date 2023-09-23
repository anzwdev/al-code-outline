export class ToolsGetFullSyntaxTreeRequest {
    source: string | undefined;
    path: string | undefined;
    projectPath: string | undefined;

    constructor(newSource: string, newPath: string, newProjectPath: string) {
        this.source = newSource;
        this.path = newPath;
        this.projectPath = newProjectPath;
    }

}