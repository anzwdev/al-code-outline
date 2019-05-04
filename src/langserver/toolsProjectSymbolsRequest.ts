export class ToolsProjectSymbolsRequest {
    includeDependencies : boolean;
    projectPath : string;
    packagesFolder : string;

    constructor(newIncludeDependencies : boolean, newProjectPath : string, newPackagesFolder : string) {
        this.includeDependencies = newIncludeDependencies
        this.projectPath = newProjectPath;
        this.packagesFolder = newPackagesFolder;
    }

}