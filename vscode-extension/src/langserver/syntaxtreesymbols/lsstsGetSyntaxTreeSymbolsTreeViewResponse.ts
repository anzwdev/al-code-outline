import { LSSTSyntaxTreeSymbolsTreeViewNode } from "./lsstsSyntaxTreeSymbolsTreeViewNode";

export interface LSSTSGetSyntaxTreeSymbolsTreeViewResponse {
    path: string | undefined;
    rootNode: LSSTSyntaxTreeSymbolsTreeViewNode | undefined;
}