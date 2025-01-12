export class ToolsGetProjectSymbolLocationRequest {
    projectPath: string;
    appFilePath: string | undefined;
    kind: string;
    name: string;

    constructor(newProjectPath: string, newAppFilePath : string | undefined, newKind: string, newName: string) {
        this.projectPath = newProjectPath;
        this.appFilePath = newAppFilePath;
        this.kind = newKind;
        this.name = newName;
    }
    
}