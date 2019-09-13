import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { SymbolsTreeView } from '../symbolstreeview/symbolsTreeView';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';

export class ALSymbolsTreeService {
    protected _context: DevToolsExtensionContext;

    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        this._context = newContext;
   
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

    }

    showEditorSymbolsTreeView() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) {
            let symbolsTreeView = new SymbolsTreeView(this._context, undefined, editor.document.uri);
            symbolsTreeView.show();
        }
    }

    async showDocumentSymbols(docUri: vscode.Uri, range: vscode.Range | undefined) {
        let symbolsTreeView = new SymbolsTreeView(this._context, undefined, docUri);
        symbolsTreeView.show();
    }

    async goToDefinitionSymbolTree() {
        let editor = vscode.window.activeTextEditor;
        if ((editor) && (editor.document) && (editor.document.uri)) {
            let locationList : vscode.Location[] | undefined = await vscode.commands.executeCommand('vscode.executeDefinitionProvider', editor.document.uri,
                editor.selection.start);
            if ((locationList) && (locationList.length > 0))
                this.showDocumentSymbols(locationList[0].uri, locationList[0].range);
        }
    }

}