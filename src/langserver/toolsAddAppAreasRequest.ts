export class ToolsAddAppAreasRequest {
    source: string;
    path: string;

    constructor(newSource: string, newPath: string) {
        this.source = newSource;
        this.path = newPath;
    }

}
