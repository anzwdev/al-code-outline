import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { ALOutlineTreeItem } from './alOutlineTreeNode';
import { ALOutlineSortMode } from './alOutlineSortMode';
import { ALOutlineTreeState } from './alOutlineTreeState';
import { ALOutlineTreeDocumentState } from './alOutlineTreeDocumentState';

export class SymbolsTreeProvider implements vscode.TreeDataProvider<ALOutlineTreeItem> {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _treeRoot : ALOutlineTreeItem | undefined;
    protected _sortMode : ALOutlineSortMode;
    protected _state: ALOutlineTreeState;
    protected _currDocState: ALOutlineTreeDocumentState | undefined;

	private _onDidChangeTreeData: vscode.EventEmitter<ALOutlineTreeItem | null> = new vscode.EventEmitter<ALOutlineTreeItem | null>();
	readonly onDidChangeTreeData: vscode.Event<ALOutlineTreeItem | null> = this._onDidChangeTreeData.event;

    constructor(context : DevToolsExtensionContext) {
        this._state = new ALOutlineTreeState();
        this._currDocState = undefined;
        this._sortMode = ALOutlineSortMode.position;
        this._toolsExtensionContext = context;        
        this._toolsExtensionContext.activeDocumentSymbols.onSymbolsChanged(symbolsLib => this.onSymbolsChanged(symbolsLib));
        this.updateSortModeState();
    }

    protected onSymbolsChanged(library : AZSymbolsLibrary) {
        if (this._treeRoot)
            this._treeRoot.saveState(this._currDocState!);

        if (library.rootSymbol) {
            let u = library.getUri();

            this._currDocState = this._state.getDocumentState(library.getSourceId());
            this._treeRoot = new ALOutlineTreeItem(library.rootSymbol, this._toolsExtensionContext.vscodeExtensionContext, undefined, this._currDocState, this._currDocState.getId(), 0);
            this._treeRoot.sort(this._sortMode);
        } else {
            this._treeRoot = undefined;
            this._currDocState = undefined;
        }

        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire(null);
    }

    public refresh() {
        this._toolsExtensionContext.activeDocumentSymbols.loadAsync(true);
    }

    public collapseAll() {
        vscode.commands.executeCommand('workbench.actions.treeView.azALDevTools.SymbolsTreeProvider.collapseAll');
    }

    //#region TreeDataProvider implementation

    getTreeItem(element: ALOutlineTreeItem): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }
    
    async getChildren(element?: ALOutlineTreeItem): Promise<ALOutlineTreeItem[]> {
        if ((element) && (element.childNodes))
            return element.childNodes;
        if ((!element) && (this._treeRoot) && (this._treeRoot.childNodes))
            return this._treeRoot.childNodes;
        return [];
    }

    getParent(element: ALOutlineTreeItem): ALOutlineTreeItem | undefined {
        return element.parent;
    }

    //#endregion

    getNodeAtPosition(position: vscode.Position): ALOutlineTreeItem | undefined {
        if (this._treeRoot)
            return this._treeRoot.findNodeAtPosition(position, false);
        return undefined;
    }

    setSortMode(mode: ALOutlineSortMode) {
        if (this._sortMode != mode) {
            this._sortMode = mode;
            if (this._treeRoot) {
                this._treeRoot.sort(this._sortMode);
                if (this._onDidChangeTreeData)
                    this._onDidChangeTreeData.fire(null);
            }
        }
        this.updateSortModeState();
    }

    private updateSortModeState() {
        let state = ALOutlineSortMode[this._sortMode];
        vscode.commands.executeCommand('setContext', 'azALDevTools:alOutlineSortMode', state);
    }

    private saveState() {
        
    }

}