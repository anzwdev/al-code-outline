export class ToolsAddAppAreasRequest {
    source: string;
    path: string;
    appAreaName: string;

    constructor(newSource: string, newPath: string, newAppAreaName: string) {
        this.source = newSource;
        this.path = newPath;
        this.appAreaName = newAppAreaName;
    }

}
