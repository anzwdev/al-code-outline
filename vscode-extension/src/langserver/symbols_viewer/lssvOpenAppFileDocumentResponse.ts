import { LSSVSymbvolsViewerNode } from "./lssvSymbolsViewerNode";

export interface LSSVOpenAppFileDocumentResponse {
    path?: string;
    documentUid?: number;
    root?: LSSVSymbvolsViewerNode;
}