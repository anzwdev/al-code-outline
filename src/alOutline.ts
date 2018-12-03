import * as vscode from 'vscode';
import * as path from 'path';
import { ALSymbolInfo } from './alSymbolInfo';
import { ALSymbolKind } from './alSymbolKind';
import { ALSourceCodeProcessor } from './alSourceCodeProcessor';

export class ALOutlineProvider implements vscode.TreeDataProvider<ALSymbolInfo> {
    private extensionContext : vscode.ExtensionContext;
	private editor: vscode.TextEditor;
    private rootNode : ALSymbolInfo;
    private autoRefresh : boolean = true;
    private appALSymbolInfo : ALSymbolInfo;
    private appALSymbolInfoWithGroups : ALSymbolInfo;
    
	private _onDidChangeTreeData: vscode.EventEmitter<ALSymbolInfo | null> = new vscode.EventEmitter<ALSymbolInfo | null>();
	readonly onDidChangeTreeData: vscode.Event<ALSymbolInfo | null> = this._onDidChangeTreeData.event;

    //------------------------------------------------------
    // initialization
    //------------------------------------------------------

    constructor(context : vscode.ExtensionContext) {
        this.extensionContext = context;

        //initialize change tracking event
		vscode.window.onDidChangeActiveTextEditor(editor => {
            if (this.autoRefresh)
                this.refresh();
        });
        
        vscode.workspace.onDidChangeTextDocument(e => this.onDocumentChanged(e));
        
        vscode.workspace.onDidCloseTextDocument(document => {
            if ((this.autoRefresh) && (this.documentEquals(document)))
                this.refresh();
        });

        vscode.workspace.onDidSaveTextDocument(document => {
            if ((this.autoRefresh) && (this.documentEquals(document)))
                this.refresh();            
        });

        vscode.workspace.onDidChangeConfiguration(data => {
            this.autoRefresh = vscode.workspace.getConfiguration('alOutline').get('autorefresh');
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
        this.appALSymbolInfoWithGroups = null;
        this.refreshALAppSymbolInfo();
        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire();
    }

    protected refreshALAppSymbolInfo() {
        let symbolInfo : ALSymbolInfo = this.appALSymbolInfo;
                
        this.rootNode = new ALSymbolInfo(null, 'al');
        this.rootNode.addChild(symbolInfo);
    
    }

    //------------------------------------------------------------------
    // tree operations
    //------------------------------------------------------------------

	async refresh(treeNode?: ALSymbolInfo): Promise<void> {
		await this.parseTree(true);
	}

    //gets document symbols using language server protocol 
    private getLspSymbols(document: vscode.TextDocument): Thenable<vscode.SymbolInformation[] | vscode.DocumentSymbol[]> {
        return vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', document.uri);
    }

    private findValidActiveEditor() : vscode.TextEditor {
        return vscode.window.activeTextEditor;
    }

    private async parseTree(triggerEvent : boolean): Promise<void> {        
        this.rootNode = null;        
        
        //try to find active text editor with file
        this.editor = this.findValidActiveEditor();            
        if (this.editor && this.editor.document)  {
            let mainTreeNode = new ALSymbolInfo(null, this.editor.document.languageId);
            //load all symbols
            let lspSymbols = await this.getLspSymbols(this.editor.document);
            if ((lspSymbols) && (lspSymbols.length > 0)) {
                let anySymbol : any = lspSymbols[0];
                if (anySymbol.children) {
                    let docSymbolList : vscode.DocumentSymbol[] = lspSymbols as vscode.DocumentSymbol[];
                    let alDocSymbols = docSymbolList.map(symbol => new ALSymbolInfo(symbol, this.editor.document.languageId));
                    for (let docSymbolIndex = 0; docSymbolIndex < alDocSymbols.length; docSymbolIndex++) {
                        mainTreeNode.addChild(alDocSymbols[docSymbolIndex]);
                    }
                } else {
                    let symbolInfoList : vscode.SymbolInformation[] = lspSymbols as vscode.SymbolInformation[];
                    let alSymbols = symbolInfoList.map(symbol => new ALSymbolInfo(symbol, this.editor.document.languageId));
                    //build symbols tree
                    mainTreeNode.appendSymbolInfoChildNodes(alSymbols);
                }
            }
            this.rootNode = mainTreeNode;

            this.appALSymbolInfo = null;
            this.appALSymbolInfoWithGroups = null;

        } else if (this.appALSymbolInfo) {
            this.refreshALAppSymbolInfo();
        } else {
            this.rootNode = new ALSymbolInfo(null, '');            
        }

        if ((triggerEvent) && (this._onDidChangeTreeData))
            this._onDidChangeTreeData.fire();
	}

    findSymbolObjectId(alSymbol : ALSymbolInfo) {
        if ((alSymbol.languageId == 'al') && (alSymbol.alElementId == 0)) {
            var editor = this.findValidActiveEditor();
            if ((editor) && (editor.document)) {
                var symbolRange = alSymbol.range;
                var textRange : vscode.Range = new vscode.Range(0, 0, symbolRange.start.line, symbolRange.start.character)
                var text : string = editor.document.getText(textRange);
                //remove comments from text
                text = ALSourceCodeProcessor.RemoveComments(text);
                //find last number
                var newObjectId = ALSourceCodeProcessor.GetLastWordAsNumber(text);
                if (newObjectId > 0)
                    alSymbol.alElementId = newObjectId;
            }
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
        if (element.selectionRange) 
            treeItem.command = {
                command: 'alOutline.openSelection',
                title: '',
                arguments: [
                    element.selectionRange
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