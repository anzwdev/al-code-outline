export class ToolsGetSyntaxTreeSymbolsRequest {
    path: string;
    symbolPath: number[];

    constructor(newPath: string, newSymbolPath: number[]) {
        this.path = newPath;
        this.symbolPath = newSymbolPath;
    }
}