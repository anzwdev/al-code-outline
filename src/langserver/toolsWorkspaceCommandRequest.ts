import { throws } from "assert";

export class ToolsWorkspaceCommandRequest {
    command: string | undefined;
    source: string | undefined;
    path: string | undefined;
    parameters: any;

    constructor(newCommand: string | undefined, newSource: string | undefined,
        newPath: string | undefined, newParameters: any) {
        this.command = newCommand;
        this.source = newSource;
        this.path = newPath;
        this.parameters = newParameters;
    }

}