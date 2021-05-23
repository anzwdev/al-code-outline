export class ToolsGetALAppContentRequest {
    appPath: string;
    filePath: string;

    constructor(newAppPath: string, newFilePath: string) {
        this.appPath = newAppPath;
        this.filePath = newFilePath;        
    }

}