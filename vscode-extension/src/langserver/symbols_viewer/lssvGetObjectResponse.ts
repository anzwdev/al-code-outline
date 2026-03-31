import { LSSVSymbvolsViewerNode } from "./lssvSymbolsViewerNode";

export interface LSSVGetObjectResponse {
    documentUid?: number;
    objectUid?: number;
    root?: LSSVSymbvolsViewerNode;
}