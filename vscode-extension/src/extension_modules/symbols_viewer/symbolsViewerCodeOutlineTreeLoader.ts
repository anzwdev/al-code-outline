import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { LSSVSymbvolsViewerNode } from "../../langserver/symbols_viewer/lssvSymbolsViewerNode";
import { CodeOutlineTreeDocumentState } from "../code_outline_view/codeOutlineTreeDocumentState";
import { CodeOutlineTreeItem } from "../code_outline_view/codeOutlineTreeItem";
import { CodeOutlineTreeLoader } from "../code_outline_view/codeOutlineTreeLoader";
import { SyntaxTreeSymbolsIcons } from "../syntax_tree_symbols/syntaxTreeSymbolsIcons";

export class SymbolsViewerCodeOutlineTreeLoader implements CodeOutlineTreeLoader {
    private _treeId: string | undefined;
    private _selectedSymbol: LSSVSymbvolsViewerNode | undefined;

    constructor(treeId: string | undefined) {
        this._treeId = treeId;
    }

    setSelectedSymbol(symbol: LSSVSymbvolsViewerNode | undefined) {
        this._selectedSymbol = symbol;
    }

    getTreeId(): string | undefined {
        return this._treeId;
    }

    async loadTree(context: DevToolsExtensionContext, state: CodeOutlineTreeDocumentState): Promise<CodeOutlineTreeItem | undefined> {
        if (this._selectedSymbol) {
            return this.createTreeItem(this._selectedSymbol, undefined, state, context);
        }        
        return undefined;
    }

    private createTreeItem(node: LSSVSymbvolsViewerNode, parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState, context: DevToolsExtensionContext): CodeOutlineTreeItem {
        let iconPath = SyntaxTreeSymbolsIcons.getIconPath(context.vscodeExtensionContext, node.kind, node.access, "");
        let item = new CodeOutlineTreeItem(node.uid.toString(), node.fullName ?? "", node.kind, undefined, undefined, iconPath, parent, state);

        if ((node.childSymbols) && (node.childSymbols.length > 0)) {
            let childSymbols: CodeOutlineTreeItem[] = [];
            for (let i=0; i<node.childSymbols.length; i++) {
                childSymbols.push(this.createTreeItem(node.childSymbols[i], item, state, context));
            }
            item.updateChildNodes(childSymbols, state);
        }

        return item;
    }

}