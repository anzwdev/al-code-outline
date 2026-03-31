import * as vscode from 'vscode';
import * as path from 'path';
import { EditorSymbolsTreeWebView } from "../../ui_core/editor_symbols_tree_webview/editorSymbolsTreeWebView";
import { CodeOutlineWebViewTreeItem } from "./codeOutlineWebViewTreeItem";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { CodeOutlineWebViewTreeProvider } from './codeOutlineWebViewTreeProvider';

export class CodeOutlineWebView extends EditorSymbolsTreeWebView<CodeOutlineWebViewTreeItem, CodeOutlineWebViewTreeProvider>{
    selectedSymbolRange?: vscode.Range;

    constructor(devToolsContext : DevToolsExtensionContext, treeItemsProvider: CodeOutlineWebViewTreeProvider) {        
        super(devToolsContext, treeItemsProvider, "azALDevTools.SymbolsTreeView");
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'symbolstreeview', 'symbolstreeview.html');
    }

}