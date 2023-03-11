import { TextRange } from "../symbollibraries/textRange";

export class ToolsWorkspaceCommandRequest {
    command: string | undefined;
    source: string | undefined;
    projectPath: string | undefined;
    filePath: string | undefined;
    range: TextRange | undefined;
    parameters: any;
    excludeFiles: string[] | undefined;

    constructor(newCommand: string | undefined, newSource: string | undefined,
        newProjectPath: string | undefined, 
        newFilePath: string | undefined,
        newRange: TextRange | undefined, 
        newParameters: any,
        newExcludeFiles: string[] | undefined) {
        this.command = newCommand;
        this.source = newSource;
        this.projectPath = newProjectPath;
        this.filePath = newFilePath;
        this.range = newRange;
        this.parameters = newParameters;
        this.excludeFiles = newExcludeFiles;
    }

}