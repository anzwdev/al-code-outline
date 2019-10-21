export class ToolsProjectSymbolsRequest {
    includeDependencies : boolean;
    projectPath : string;
    packagesFolder : string;
    workspaceFolders : string[];

    constructor(newIncludeDependencies : boolean, newProjectPath : string, newPackagesFolder : string, newWorkspaceFolders : string[]) {
        this.includeDependencies = newIncludeDependencies
        this.projectPath = newProjectPath;
        this.packagesFolder = newPackagesFolder;
        this.workspaceFolders = newWorkspaceFolders;
    }

}