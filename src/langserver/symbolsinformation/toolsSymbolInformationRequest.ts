export class ToolsSymbolInformationRequest {
    path: string | undefined;
    includeNonAccessible: boolean;

    constructor(newPath: string | undefined, newIncludeNonAccessible: boolean) {
        this.path = newPath;
        this.includeNonAccessible = newIncludeNonAccessible;
    }

}