import * as vscode from 'vscode';
import { DocToolsWebView } from "../docToolsWebView";
import { TextEditorHelper } from "../textEditorHelper";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { EditorSymbolsTreeItem } from './editorSymbolsTreeItem';
import { EditorSymbolsTreeProvider } from './editorSymbolsTreeProvider';

export class EditorSymbolsTreeWebView<TItem extends EditorSymbolsTreeItem, TProvider extends EditorSymbolsTreeProvider<TItem>> extends DocToolsWebView {
    protected treeItemsProvider: TProvider;
    protected selectedItem?: TItem;

    constructor(devToolsContext : DevToolsExtensionContext, treeItemsProvider: TProvider, viewType: string) {
        super(devToolsContext, treeItemsProvider.documentName, treeItemsProvider.documentUri, viewType);

        this.treeItemsProvider = treeItemsProvider;
        this.selectedItem = undefined;

        this.subscriptions.push(this.treeItemsProvider.onDidChange(() => {
            this.onTreeChanged();
        }));
    }

    protected async onDocumentLoaded() {
        this._loaded = true;
        await this.loadSymbols();
    }

    protected async loadSymbols() {
        await this.treeItemsProvider.load();
    }

    protected onTreeChanged() {
        this.selectedItem = undefined;
        this.sendSetDataMessage();
    }

    protected sendSetDataMessage() {
        this.sendMessage({
            command: 'setData',
            data: this.treeItemsProvider.getRootItem(),
            selected: this.selectedItem
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        if (message) {
            switch (message.command) {
                case 'definition':
                    if (message.uid) {
                        this.goToDefinition(message.uid);
                    }
                    return true;
                case 'symbolselected':
                    if (message.uid) {
                        this.selectSymbolByUid(message.uid);
                    }
                    return true;
                case 'refresh':
                    this.loadSymbols();
                    return true;
                case 'sync':
                    this.selectSymbolAtCursor();
                    return true;
                }
        }

        return false;
    }

    protected symbolSelectedByUid(uid: string | undefined) {
        this.selectedItem = this.treeItemsProvider.findByUid(uid);
        this.onSymbolSelected();
    }

    protected selectSymbolByUid(uid: string | undefined) {
        let item = this.treeItemsProvider.findByUid(uid);
        this.selectSymbol(item);
    }

    selectSymbolInRange(range: vscode.Range) {
        this.selectSymbol(this.treeItemsProvider.findByRange(range));
    }

    selectSymbolAtCursor() {
        if (this._documentUri) {
            let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
            if (editor) {
                this.selectSymbolInRange(editor.selection);
            }
        }
    }

    protected selectSymbol(item: TItem | undefined) {
        this.selectedItem = item;
        if (this.selectedItem) {
            this.sendMessage({
                command: 'selectSymbol',
                selected: this.selectedItem
            });
        }
        this.onSymbolSelected();
    }

    protected async onSymbolSelected() {
    }

    protected async goToDefinition(uid: string | undefined) {
        let item = this.treeItemsProvider.findByUid(uid);
        if ((item?.selectionRange) && (this.treeItemsProvider.documentUri)) {
            TextEditorHelper.openEditor(this.treeItemsProvider.documentUri, true, true, new vscode.Position(item.selectionRange.start.line, 
                item.selectionRange.start.character));
        }
    }

    protected selectCurrentSymbolInEditor() {
        if (this.selectedItem?.selectionRange) {
            let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
            if (editor) {
                editor.selection = new vscode.Selection(this.selectedItem.selectionRange.start, this.selectedItem.selectionRange.end);
                editor.revealRange(editor.selection);
            }
        }
    }

    protected override onPanelClosed(): void {
        super.onPanelClosed();
        this.treeItemsProvider.close();
    }
 

}