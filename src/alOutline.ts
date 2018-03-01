import * as vscode from 'vscode';
import * as path from 'path';
import { ALSymbolInfo } from './alSymbolInfo';
import { ALSymbolKind } from './alSymbolKind';

export class ALOutlineProvider implements vscode.TreeDataProvider<ALSymbolInfo> {
    private extensionContext : vscode.ExtensionContext;
	private editor: vscode.TextEditor;
    private rootNode : ALSymbolInfo;
    private autoRefresh : boolean = true;
    private parserActive : boolean = false;
    private appALSymbolInfo : ALSymbolInfo;
    
	private _onDidChangeTreeData: vscode.EventEmitter<ALSymbolInfo | null> = new vscode.EventEmitter<ALSymbolInfo | null>();
	readonly onDidChangeTreeData: vscode.Event<ALSymbolInfo | null> = this._onDidChangeTreeData.event;

    //------------------------------------------------------
    // initialization
    //------------------------------------------------------

    constructor(context : vscode.ExtensionContext) {
        this.extensionContext = context;

        //initialize change tracking event
	//	vscode.window.onDidChangeActiveTextEditor(editor => {
    //        if (this.autoRefresh)
    //            this.refresh();
    //    });
        
        vscode.workspace.onDidChangeTextDocument(e => this.onDocumentChanged(e));
        
        vscode.workspace.onDidCloseTextDocument(document => {
            if ((this.autoRefresh) && (this.documentEquals(document)))
                this.refresh();
        });

        vscode.workspace.onDidSaveTextDocument(document => {
            if ((this.autoRefresh) && (this.documentEquals(document)))
                this.refresh();            
        });

        vscode.window.onDidChangeVisibleTextEditors(e => {
            if (this.autoRefresh)
                this.refresh();
        });

		this.autoRefresh = vscode.workspace.getConfiguration('alOutline').get('autorefresh');

		this.parseTree(false);
    }    

	private onDocumentChanged(changeEvent: vscode.TextDocumentChangeEvent): void {
        if ((this.autoRefresh) && (this.documentEquals(changeEvent.document)))
            this.refresh();
    }
    
	select(range: vscode.Range) {
        this.editor.revealRange(range, vscode.TextEditorRevealType.Default);
        this.editor.selection = new vscode.Selection(range.start, range.end);
        vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');
    }
    
    private documentEquals(document : vscode.TextDocument) : boolean {
        return ((document) && (this.editor) && (this.editor.document) && document.uri.toString() === this.editor.document.uri.toString());
    }

    setAppALSymbolInfo(newSymbolInfo : ALSymbolInfo) {
        this.appALSymbolInfo = newSymbolInfo;
        this.rootNode = new ALSymbolInfo(null, 'al');
        this.rootNode.addChild(newSymbolInfo);
        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire();
    }

    //------------------------------------------------------------------
    // tree operations
    //------------------------------------------------------------------

	async refresh(treeNode?: ALSymbolInfo): Promise<void> {
		await this.parseTree(true);
	}

    //gets document symbols using language server protocol 
    private getLspSymbols(document: vscode.TextDocument): Thenable<vscode.SymbolInformation[]> {
        return vscode.commands.executeCommand<vscode.SymbolInformation[]>('vscode.executeDocumentSymbolProvider', document.uri);
    }

    private isValidTextEditor(textEditor : vscode.TextEditor) {
        return ((textEditor) && 
            (textEditor.document) && 
            (textEditor.document.uri) && 
            (textEditor.document.uri.scheme !== 'debug') &&
            (textEditor.document.uri.scheme !== 'output'));
    }

    private findValidActiveEditor() : vscode.TextEditor {
        let activeEditor = vscode.window.activeTextEditor;
        if (this.isValidTextEditor(activeEditor))
            return activeEditor;
        
        let visibleEditors = vscode.window.visibleTextEditors;
        if (visibleEditors) {
            var i : number;
            for (i=0; i<visibleEditors.length; i++) {
                if (this.isValidTextEditor(visibleEditors[i]))
                    return visibleEditors[i];
            }
        }
        return null;
    }

    private async parseTree(triggerEvent : boolean): Promise<void> {        
        if (!this.parserActive)
        {
            this.parserActive = true;
            this.rootNode = null;        

            //try to find active text editor with file
            this.editor = this.findValidActiveEditor();            
            if (this.editor && this.editor.document)  {
                let mainTreeNode = new ALSymbolInfo(null, this.editor.document.languageId);
                //load all symbols
                let lspSymbols = await this.getLspSymbols(this.editor.document);
                let alSymbols = lspSymbols.map(symbol => new ALSymbolInfo(symbol, this.editor.document.languageId));
                //build symbols tree
                mainTreeNode.appendChildNodes(alSymbols);

                this.rootNode = mainTreeNode;
            } else if (this.appALSymbolInfo) {
                this.rootNode = new ALSymbolInfo(null, 'al');
                this.rootNode.addChild(this.appALSymbolInfo);
            } else {
                this.rootNode = new ALSymbolInfo(null, '');            
            }

            if ((triggerEvent) && (this._onDidChangeTreeData))
                this._onDidChangeTreeData.fire();
            
            this.parserActive = false;
        }
	}

    //------------------------------------------------------------------
    // TreeDataProvider implementation
    //------------------------------------------------------------------

    getTreeItem(element: ALSymbolInfo): vscode.TreeItem | Thenable<vscode.TreeItem> {
        let treeItem = new vscode.TreeItem(element.name);
        treeItem.iconPath = element.getIcon(this.extensionContext);
        //expand
        if (element.childItems.length) {
            treeItem.collapsibleState = vscode.TreeItemCollapsibleState.Expanded;
        } else {
            treeItem.collapsibleState = vscode.TreeItemCollapsibleState.None;
        }
        //node command
        if (element.lspSymbol) 
            treeItem.command = {
                command: 'alOutline.openSelection',
                title: '',
                arguments: [
                    element.lspSymbol.location.range
                ]
            };
        //node context
        treeItem.contextValue = element.getKindName();

        return treeItem;
    }
    
    async getChildren(element?: ALSymbolInfo): Promise<ALSymbolInfo[]> {
        if (element)
            return element.childItems;
        return this.rootNode ? this.rootNode.childItems : [];
    }


}