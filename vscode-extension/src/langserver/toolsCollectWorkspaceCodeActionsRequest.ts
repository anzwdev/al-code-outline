import { TextRange } from "../symbollibraries/textRange";

export class ToolsCollectWorkspaceCodeActionsRequest {
    source: string | undefined;
    projectPath: string | undefined;
    filePath: string | undefined;
    range: TextRange | undefined;

    constructor(newSource: string | undefined,
        newProjectPath: string | undefined, 
        newFilePath: string | undefined,
        newRange: TextRange | undefined) {
        this.source = newSource;
        this.projectPath = newProjectPath;
        this.filePath = newFilePath;
        this.range = newRange;
    }

}