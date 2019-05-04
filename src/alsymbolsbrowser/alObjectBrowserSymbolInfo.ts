import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from "../symbollibraries/azSymbolKind";

export class ALObjectBrowserSymbolInfo {
    id : number;
    idx: number;
    name : string;
    kind : AZSymbolKind; 
    library: string;
    childSymbols: ALObjectBrowserSymbolInfo[] | undefined;  

    constructor(symbol: AZSymbolInformation, libraryName: string) {
        this.id = symbol.id;
        this.idx = symbol.idx;
        this.name = symbol.name;
        this.kind = symbol.kind;
        this.library = libraryName;
        this.childSymbols = undefined;
    }

    addChildSymbol(symbol: ALObjectBrowserSymbolInfo) {
        if (!this.childSymbols)
            this.childSymbols = [];
        this.childSymbols.push(symbol);
    }

}