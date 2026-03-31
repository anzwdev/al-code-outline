import * as vscode from 'vscode';
import { CodeOutlineTreeItem } from './codeOutlineTreeItem';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { CodeOutlineTreeState } from './codeOutlineTreeState';
import { CodeOutlineTreeDocumentState } from './codeOutlineTreeDocumentState';
import { CodeOutlineTreeLoader } from './codeOutlineTreeLoader';
import { ALCodeOutlineTreeLoader } from './alCodeOutlineTreeLoader';
import { DocSymbolsCodeOutlineTreeLoader } from './docSymbolsCodeOutlineTreeLoader';
import { CodeOutlineSortMode } from './codeOutlineSortMode';
import { CodeOutlineViewConst } from './codeOutlineViewConst';

export class CodeOutlineTreeProvider implements vscode.TreeDataProvider<CodeOutlineTreeItem> {

    private _context: DevToolsExtensionContext;

	private _onDidChangeTreeData: vscode.EventEmitter<CodeOutlineTreeItem | null> = new vscode.EventEmitter<CodeOutlineTreeItem | null>();
	readonly onDidChangeTreeData: vscode.Event<CodeOutlineTreeItem | null> = this._onDidChangeTreeData.event;
    private _treeState: CodeOutlineTreeState;
    private _curretDocumentTreeState?: CodeOutlineTreeDocumentState;
    private _rootNode?: CodeOutlineTreeItem;
    private _sortMode: CodeOutlineSortMode = CodeOutlineSortMode.position;

    private _currentTreeLoader?: CodeOutlineTreeLoader;
    private _documentTreeLoader: DocSymbolsCodeOutlineTreeLoader;
    private _alTreeLoader: ALCodeOutlineTreeLoader;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
        this._treeState = new CodeOutlineTreeState();
        this._curretDocumentTreeState = undefined;

        this._documentTreeLoader = new DocSymbolsCodeOutlineTreeLoader();
        this._alTreeLoader = new ALCodeOutlineTreeLoader();
        this._currentTreeLoader = undefined;
        
        context.syntaxTreeSymbolsService.onDidActiveDocumentContentChanged(() => {
            this.refresh();
        });

        this.updateSortModeState();

        // Initial load (startup)
        void this.refresh();

        // VS Code may restore editors after activation; run one more refresh.
        setTimeout(() => {
            this.refresh();
        }, 250);        
    }

    public async setTreeLoader(loader: CodeOutlineTreeLoader | undefined) {
        this._currentTreeLoader = loader;

        if (this._currentTreeLoader) {
            let sourceId = this._currentTreeLoader.getTreeId() ?? "undefined";
            this._curretDocumentTreeState = this._treeState.getDocumentState(sourceId);
            this._rootNode = await this._currentTreeLoader.loadTree(this._context, this._curretDocumentTreeState);
        } else {
            this._rootNode = undefined;
            this._curretDocumentTreeState = undefined;
        }

        if (this._onDidChangeTreeData) {
            this._onDidChangeTreeData.fire(null);
        }
    }

    public async refresh() {
        let treeLoader: CodeOutlineTreeLoader | undefined = undefined;
        
        if (vscode.window.activeTextEditor?.document) {
            let languageId = vscode.window.activeTextEditor.document.languageId;
            if (languageId === "al") {
                treeLoader = this._alTreeLoader;
            } else {
                treeLoader = this._documentTreeLoader;
            }
        } else {
            treeLoader = undefined;
        }

        await this.setTreeLoader(treeLoader);
    }

    public collapseAll() {
        vscode.commands.executeCommand('workbench.actions.treeView.azALDevTools.SymbolsTreeProvider.collapseAll');
    }

    getTreeItem(element: CodeOutlineTreeItem): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }
    
    async getChildren(element?: CodeOutlineTreeItem): Promise<CodeOutlineTreeItem[]> {
        if (element) {
            return element.childNodes ?? [];
        }       
        return this._rootNode?.childNodes ?? [];
    }

    getParent(element: CodeOutlineTreeItem): CodeOutlineTreeItem | undefined {
        return element.parent;
    }

    getNodeAtPosition(position: vscode.Position): CodeOutlineTreeItem | undefined {
        if (this._rootNode) {
            return this._rootNode.findNodeAtPosition(position, false);
        }
        return undefined;
    }

    public setNodeState(element: CodeOutlineTreeItem, state: vscode.TreeItemCollapsibleState) {
        element.collapsibleState = state; // optional mirror
        if (this._curretDocumentTreeState && element.id) {
            this._curretDocumentTreeState.setState(element.id, state);
        }
    }    

    setSortMode(mode: CodeOutlineSortMode) {
        if (this._sortMode !== mode) {
            this._sortMode = mode;
            if (this._rootNode) {
                this._rootNode.sort(this._sortMode);

                if (this._onDidChangeTreeData) {
                    this._onDidChangeTreeData.fire(null);
                }
            }
        }
        this.updateSortModeState();
    }

    private updateSortModeState() {
        let state = CodeOutlineSortMode[this._sortMode];
        vscode.commands.executeCommand('setContext', CodeOutlineViewConst.ctxSortMode, state);
    }

}