import { AZSymbolKind } from "../symbollibraries/azSymbolKind";

export class ToolsLibrarySymbolsDetailsRequest {
    libraryId: number;
    kind: AZSymbolKind;
    paths: number[][];

    constructor(newLibraryId : number, newKind: AZSymbolKind, newPaths: number[][]) {
        this.libraryId = newLibraryId;
        this.kind = newKind;
        this.paths = newPaths;
    }

}