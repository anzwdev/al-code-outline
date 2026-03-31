import { LSSymbolsViewerClient } from "../../langserver/symbols_viewer/lsSymbolsViewerClient";
import { SymbolsViewerDocument } from "./symbolsViewerDocument";

export interface SymbolsViewerLoader {
    
    lsClient: LSSymbolsViewerClient;
    load(): Promise<SymbolsViewerDocument | undefined>;
    getName(): string;

}