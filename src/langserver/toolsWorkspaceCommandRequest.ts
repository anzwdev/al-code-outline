import { TextRange } from "../symbollibraries/textRange";

export class ToolsWorkspaceCommandRequest {
    command: string | undefined;
    source: string | undefined;
    path: string | undefined;
    range: TextRange | undefined;
    parameters: any;

    constructor(newCommand: string | undefined, newSource: string | undefined,
        newPath: string | undefined, newRange: TextRange | undefined, newParameters: any) {
        this.command = newCommand;
        this.source = newSource;
        this.path = newPath;
        this.range = newRange;
        this.parameters = newParameters;
    }

}