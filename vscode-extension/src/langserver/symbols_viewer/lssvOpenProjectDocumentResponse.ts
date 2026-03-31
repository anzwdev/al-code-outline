import { LSSVSymbvolsViewerNode } from "./lssvSymbolsViewerNode";

export interface LSSVOpenProjectDocumentResponse {
    path?: string;
    includeDependencies?: boolean;
    documentUid?: number;
    root?: LSSVSymbvolsViewerNode;
}