import { AZSymbolKind } from "../symbollibraries/azSymbolKind";

export class ALObjectBrowserItem {
    public kind: AZSymbolKind;
    public id: number;
    public name: string;
    public library: string;
    public path: number[];

    constructor(newKind: AZSymbolKind, newId: number, newName: string, newLibrary: string, newPath: number[]) {
        this.kind = newKind;
        this.id = newId;
        this.name = newName;
        this.library = newLibrary;
        this.path = newPath;
    }

}