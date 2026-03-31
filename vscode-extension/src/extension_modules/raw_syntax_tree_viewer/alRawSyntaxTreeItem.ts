import { EditorSymbolsTreeItem } from "../../ui_core/editor_symbols_tree_webview/editorSymbolsTreeItem";

export class ALRawSyntaxTreeItem extends EditorSymbolsTreeItem { 
    type?: string;
    childSymbols?: ALRawSyntaxTreeItem[];
}
