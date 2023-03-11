export class ToolsGetLibrarySymbolLocationRequest {
    libraryId: number;
    path: number[];

    constructor(newLibraryId: number, newPath: number[]) {
        this.libraryId = newLibraryId;
        this.path = newPath;
    }

}