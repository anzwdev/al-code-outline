import { AZSymbolsLibrary } from "./azSymbolsLibrary";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolKind } from "./azSymbolKind";
import { AZSymbolInformation } from "./azSymbolInformation";
import { ToolsLibrarySymbolsDetailsRequest } from "../langserver/toolsLibrarySymbolsDetailsRequest";
import { ToolsCloseSymbolsLibraryRequest } from "../langserver/toolsCloseSymbolsLibraryRequest";
import { ALSymbolSourceLocation } from "./alSymbolSourceLocation";
import { ToolsGetLibrarySymbolLocationRequest } from "../langserver/toolsGetLibrarySymbolLocationRequest";

export class ALBaseServerSideLibrary extends AZSymbolsLibrary {
    protected _context : DevToolsExtensionContext;
    protected _libraryId : number;

    constructor(context : DevToolsExtensionContext) {
        super();
        this._context = context;
        this._libraryId = 0;
    }

    public async getSymbolsListByPathAsync(pathList: number[][], kind : AZSymbolKind) : Promise<AZSymbolInformation[]> {       
        let data = await this._context.toolsLangServerClient.getLibrarySymbolsDetails(
            new ToolsLibrarySymbolsDetailsRequest(this._libraryId, kind, pathList)
        );
        let symbolList : AZSymbolInformation[] = [];

        if ((data) && (data.symbols) && (data.symbols.length > 0)) {
            for (let i=0; i<data.symbols.length; i++) {
                let symbol = AZSymbolInformation.fromAny(data.symbols[i]);
                if (symbol) {
                    symbol.updateTree(true, this._twoWayTree);
                    symbolList.push(symbol);
                }
            }
        }

        return symbolList;
    }

    public async getSymbolLocationByPath(symbolPath: number[]) : Promise<ALSymbolSourceLocation | undefined> {
        let data = await this._context.toolsLangServerClient.getLibrarySymbolLocation(
            new ToolsGetLibrarySymbolLocationRequest(this._libraryId, symbolPath));
        if (data)
            return data.location;
        return undefined;
    }

    public async unloadAsync() {
        this._context.toolsLangServerClient.closeSymbolsLibrary(new ToolsCloseSymbolsLibraryRequest(this._libraryId));
    }

}