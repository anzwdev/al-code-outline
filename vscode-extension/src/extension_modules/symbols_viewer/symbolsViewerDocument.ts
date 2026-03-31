import { LSSVSymbvolsViewerNode } from "../../langserver/symbols_viewer/lssvSymbolsViewerNode";

export interface SymbolsViewerDocument {
    uid: number;
    root?: LSSVSymbvolsViewerNode;
}