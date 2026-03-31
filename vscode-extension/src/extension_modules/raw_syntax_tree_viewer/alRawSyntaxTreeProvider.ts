import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { EditorSymbolsTreeProvider } from "../../ui_core/editor_symbols_tree_webview/editorSymbolsTreeProvider";
import { LSRSTVRawSyntaxTreeViewerNode } from '../../langserver/raw_syntax_tree_viewer/lsrstvRawSyntaxTreeViewerNode';
import { ALRawSyntaxTreeItem } from './alRawSyntaxTreeItem';
import { LSTextRangeToVSCRangeConverter } from '../../langserver/common_types/lsTextRangeToVSCRangeConverter';
import { LSRSTVRawSyntaxTreeViewerClient } from '../../langserver/raw_syntax_tree_viewer/lsrstvRawSyntaxTreeViewerClient';
import { LSRSTVViewMode } from '../../langserver/raw_syntax_tree_viewer/lsrstvViewMode';
import { LSRSTVSyntaxTreeViewerTreeNodeProperty } from '../../langserver/raw_syntax_tree_viewer/lsrstvSyntaxTreeViewerTreeNodeProperty';

export class ALRawSyntaxTreeProvider extends EditorSymbolsTreeProvider<ALRawSyntaxTreeItem> {
    private _viewMode: LSRSTVViewMode;
    private _lsClient: LSRSTVRawSyntaxTreeViewerClient;

    constructor(context: DevToolsExtensionContext, documentUri?: vscode.Uri, documentName?: string) {
        super(context, documentUri, documentName);

        this._viewMode = LSRSTVViewMode.classView;
        this._lsClient = new LSRSTVRawSyntaxTreeViewerClient(this.context.lsConnector);
    }

    setViewMode(viewMode: LSRSTVViewMode) {
        if (this._viewMode !== viewMode) {
            this._viewMode = viewMode;
            this.load();
        }
    }

    getViewMode() : LSRSTVViewMode {
        return this._viewMode;
    }

    override getChildren(symbol: ALRawSyntaxTreeItem): ALRawSyntaxTreeItem[] | undefined {
        return symbol.childSymbols;
    }

    protected override async loadItemsFromSource() : Promise<ALRawSyntaxTreeItem | undefined> { 
        if (!this.documentUri) {
            return undefined;
        }

        let content = await this._lsClient.getSyntaxTreeViewerTreeView({ path: this.documentUri.fsPath, viewMode: this._viewMode });
        if (content?.rootNode) {
            return this.createTreeItem(content.rootNode);
        }
        return undefined;
    }

    private createTreeItem(source: LSRSTVRawSyntaxTreeViewerNode): ALRawSyntaxTreeItem {
        let item = new ALRawSyntaxTreeItem();

        item.name = source.name;
        item.fullName = source.fullName ?? source.name;
        item.uid = source.uid;
        item.idx = source.idx;
        item.range = LSTextRangeToVSCRangeConverter.convert(source.range);
        item.selectionRange = LSTextRangeToVSCRangeConverter.convert(source.selectionRange);
        item.type = source.type;
        
        if (source.childSymbols) {
            item.childSymbols = source.childSymbols.map(child => this.createTreeItem(child));
        }

        return item;
    }

    async getItemProperties(uid: string): Promise<LSRSTVSyntaxTreeViewerTreeNodeProperty[] | undefined> {
        if (!this.documentUri) {
            return undefined;
        }

        let response = await this._lsClient.getSyntaxTreeViewerTreeNodeProperties({ path: this.documentUri.fsPath, uid: uid });
        return response?.properties;
    }

}
