import * as path from 'path';
import { EditorSymbolsTreeWebView } from "../../ui_core/editor_symbols_tree_webview/editorSymbolsTreeWebView";
import { LSRSTVViewMode } from '../../langserver/raw_syntax_tree_viewer/lsrstvViewMode';
import { ALRawSyntaxTreeItem } from './alRawSyntaxTreeItem';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALRawSyntaxTreeProvider } from './alRawSyntaxTreeProvider';

export class ALRawSyntaxTreeViewer extends EditorSymbolsTreeWebView<ALRawSyntaxTreeItem, ALRawSyntaxTreeProvider> {

    constructor(devToolsContext : DevToolsExtensionContext, treeItemsProvider: ALRawSyntaxTreeProvider) {
        super(devToolsContext, treeItemsProvider, "azALDevTools.SymbolsTreeView");
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'syntaxtreeview', 'syntaxtreeview.html');
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;        
        }

        if (message) {
            switch (message.command) {
                case 'treeview':
                    this.treeItemsProvider.setViewMode(LSRSTVViewMode.treeView);
                    return true;
                case 'classview':
                    this.treeItemsProvider.setViewMode(LSRSTVViewMode.classView);
                    return true;
                }
        }

        return false;
    }

    protected async onSymbolSelected() {
        if (this.selectedItem?.uid) {
            let symbolProperties = await this.treeItemsProvider.getItemProperties(this.selectedItem.uid);
            this.setSymbolInfo(symbolProperties);
        } else {
            this.setSymbolInfo(undefined);
        }
    }

    setSymbolInfo(properties: any) {
        this.sendMessage({
            command: 'setSymbolInfo',
            data: {
                name: this.selectedItem?.name,
                type: this.selectedItem?.type,
                properties: properties
            }
        });
    }

}