import * as vscode from 'vscode';
import { EditorSymbolsTreeItem } from "./editorSymbolsTreeItem";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class EditorSymbolsTreeProvider<T extends EditorSymbolsTreeItem> implements vscode.Disposable {
    documentUri?: vscode.Uri;
    documentName?: string;

    readonly onDidChange: vscode.Event<T | undefined>;
    private _onDidChangeEventEmitter: vscode.EventEmitter<T | undefined>;
    private _rootItem?: T;
    private _itemsByUid: Map<string, T>;    

    protected subscriptions: vscode.Disposable[] = [];
    protected context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext, documentUri?: vscode.Uri, documentName?: string) {
        this.context = context;
        this.documentUri = documentUri;
        this.documentName = documentName;
        this._itemsByUid = new Map<string, T>();
        this._rootItem = undefined;

        this._onDidChangeEventEmitter = new vscode.EventEmitter<T | undefined>();
        this.onDidChange = this._onDidChangeEventEmitter.event;
    }

    async load() {
        let rootItem = await this.loadItemsFromSource();
        this.setRootItem(rootItem); 
    }

    protected async loadItemsFromSource() : Promise<T | undefined> {
        return undefined;
    }
    
    public getRootItem(): T | undefined {
        return this._rootItem;
    }

    public getChildren(symbol: T): T[] | undefined {
        return undefined;
    }

    public findByUid(uid: string | undefined): T | undefined {
        if (!uid) {
            return undefined;
        }
        return this._itemsByUid.get(uid);
    }

    public findByRange(range: vscode.Range): T | undefined {
        if (!this._rootItem) {
            return undefined;
        }
        return this.findByRangeInTree(this._rootItem, range);
    }

    private findByRangeInTree(symbol: T, range: vscode.Range): T | undefined {
        //check children first
        let children = this.getChildren(symbol);
        if (children) {
            for (let i=0; i<children.length; i++) {
                let child = children[i];
                let found = this.findByRangeInTree(child, range);
                if (found) {
                    return found;
                }
            }
        }
        //then check current symbol
        if ((symbol.range) && (symbol.range.contains(range))) {
            return symbol;
        }
        return undefined;
    }

    protected setRootItem(rootSymbol: T | undefined) {
        this._rootItem = rootSymbol;
        this.updateUidMap();
        this.notifyContentChanged();
    }

    private notifyContentChanged() {
        if (this._onDidChangeEventEmitter) {
            this._onDidChangeEventEmitter.fire(this.getRootItem());
        }   
    }

    private updateUidMap() {
        this._itemsByUid.clear();
        if (this._rootItem) {
            this.addToUidMap(this._rootItem);
        }
    }

    private addToUidMap(symbol: T) {
        if (symbol.uid !== undefined) {
            this._itemsByUid.set(symbol.uid, symbol);
        }
        let children = this.getChildren(symbol);
        if (children) {
            for (let i=0; i<children.length; i++) {
                this.addToUidMap(children[i]);
            }
        }
    }

    public close() {
    }

    dispose() {
        for (let i = 0; i < this.subscriptions.length; i++) {
            this.subscriptions[i].dispose();
        }
        this.subscriptions = [];
    }

}