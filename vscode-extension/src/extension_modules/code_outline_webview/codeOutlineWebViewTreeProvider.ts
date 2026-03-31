import * as vscode from 'vscode';
import { EditorSymbolsTreeProvider } from "../../ui_core/editor_symbols_tree_webview/editorSymbolsTreeProvider";
import { CodeOutlineWebViewTreeItem } from "./codeOutlineWebViewTreeItem";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { LSSTSyntaxTreeSymbolsTreeViewNode } from '../../langserver/syntaxtreesymbols/lsstsSyntaxTreeSymbolsTreeViewNode';
import { LSSyntaxNodeKindHelper } from '../../langserver/common_types/lsSyntaxNodeKindHelper';
import { LSTextRangeToVSCRangeConverter } from '../../langserver/common_types/lsTextRangeToVSCRangeConverter';

export class CodeOutlineWebViewTreeProvider extends EditorSymbolsTreeProvider<CodeOutlineWebViewTreeItem> {

    constructor(context: DevToolsExtensionContext, documentUri?: vscode.Uri, documentName?: string) {
        super(context, documentUri, documentName);
    }

    override getChildren(symbol: CodeOutlineWebViewTreeItem): CodeOutlineWebViewTreeItem[] | undefined {
        return symbol.childSymbols;
    }

    protected override async loadItemsFromSource() : Promise<CodeOutlineWebViewTreeItem | undefined> { 
        if (!this.documentUri) {
            return undefined;
        }

        let symbols = await this.context.syntaxTreeSymbolsService.getContent(this.documentUri);
        if (symbols) {
            return this.createTreeItem(symbols);
        }
        return undefined;
    }

    private createTreeItem(source: LSSTSyntaxTreeSymbolsTreeViewNode): CodeOutlineWebViewTreeItem {
        let item = new CodeOutlineWebViewTreeItem(source.kind);

        item.name = source.name;
        item.fullName = source.fullName ?? source.name;
        item.uid = source.uid;
        item.id = ((source.id) && (source.id !== 0)) ? source.id.toString() : undefined;
        item.icon = LSSyntaxNodeKindHelper.getIconName(source.kind, source.access, source.subtype);
        item.range = LSTextRangeToVSCRangeConverter.convert(source.range);
        item.selectionRange = LSTextRangeToVSCRangeConverter.convert(source.selectionRange);
        
        if (source.childSymbols) {
            item.childSymbols = source.childSymbols.map(child => this.createTreeItem(child));
        }

        return item;
    }

}