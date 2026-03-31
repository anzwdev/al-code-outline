import { LSSyntaxNodeKind } from "../../langserver/common_types/lsSyntaxNodeKind";
import { EditorSymbolsTreeItem } from "../../ui_core/editor_symbols_tree_webview/editorSymbolsTreeItem";

export class CodeOutlineWebViewTreeItem extends EditorSymbolsTreeItem {
    kind: LSSyntaxNodeKind;
    icon?: string;
    childSymbols?: CodeOutlineWebViewTreeItem[];

    constructor(kind: LSSyntaxNodeKind) {
        super();

        this.kind = kind;
    }


}