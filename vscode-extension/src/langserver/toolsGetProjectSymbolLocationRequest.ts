export class ToolsGetProjectSymbolLocationRequest {
    projectPath: string;
    kind: string;
    name: string;

    constructor(newProjectPath: string, newKind: string, newName: string) {
        this.projectPath = newProjectPath;
        this.kind = newKind;
        this.name = newName;
    }
    
}