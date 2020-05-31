import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { SymbolsTreeView } from '../symbolstreeview/symbolsTreeView';
import { SyntaxTreeView } from '../syntaxtreeview/syntaxTreeView';

export class ALSymbolsTreeService {
    protected _context: DevToolsExtensionContext;
    protected _uriSymbolTreeViews : { [id: string] : SymbolsTreeView | undefined };

    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        this._context = newContext;
        this._uriSymbolTreeViews = {};

        //register commands
        this.registerCommands();
    }

    protected registerCommands() {
        let that = this;

        //outline preview window
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showDocumentSymbols',
                () => that.showEditorSymbolsTreeView()
            )
        );

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.goToDefinitionSymbolTree',
                () => that.goToDefinitionSymbolTree()
            )
        )

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showSyntaxTree',
                () => that.showSyntaxTreeAnalyzer()
            )
        );

    }

    showSyntaxTreeAnalyzer() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) {
            let syntaxTree: SyntaxTreeView = new SyntaxTreeView(this._context, editor.document.uri);
            syntaxTree.show();         
        }   
    }

    showEditorSymbolsTreeView() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) 
            this.showDocumentSymbols(editor.document.uri, new vscode.Range(editor.selection.start, editor.selection.end));        
    }

    async showDocumentSymbols(docUri: vscode.Uri, range: vscode.Range | undefined) {
        let symbolsTreeView : SymbolsTreeView | undefined = this._uriSymbolTreeViews[docUri.toString()];
        if (!symbolsTreeView) {
            symbolsTreeView = new SymbolsTreeView(this._context, undefined, docUri);
            symbolsTreeView.selectedSymbolRange = range;
            this.addUriSymbolsTreeView(docUri, symbolsTreeView);
            symbolsTreeView.show();
        } else {
            symbolsTreeView.reveal();
            if (range)
                symbolsTreeView.selectSymbolInRange(range);
        }
    }

    async goToDefinitionSymbolTree() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) {
            let locationList : vscode.Location[] | undefined = await vscode.window.withProgress<vscode.Location[] | undefined>({
                location: vscode.ProgressLocation.Notification,
                title: 'Loading object definition'
            }, async (progress) => {
                return await vscode.commands.executeCommand<vscode.Location[] | undefined>('vscode.executeDefinitionProvider', editor!.document.uri,
                    editor!.selection.start);
            });

            if ((locationList) && (locationList.length > 0)) {
                let range = locationList[0].range;               
                this.showDocumentSymbols(locationList[0].uri, range);
            } else
                this.showDocumentSymbols(editor.document.uri, undefined);
        }
    }



    addUriSymbolsTreeView(uri: vscode.Uri, symbolsTreeView: SymbolsTreeView) {
        this._uriSymbolTreeViews[uri.toString()] = symbolsTreeView;
    }

    removeUriSymbolsTreeView(uri: vscode.Uri) {
        this._uriSymbolTreeViews[uri.toString()] = undefined;
    }

}