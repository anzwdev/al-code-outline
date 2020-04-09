import * as vscode from 'vscode';
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { ToolsDocumentSymbolsRequest } from '../../langserver/toolsDocumentSymbolsRequest';
import { ToolsDocumentSymbolsResponse } from '../../langserver/toolsDocumentSymbolsResponse';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';

export class ALSortPropertiesCommand extends ALBaseSortCodeCommand {

    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALSortPropertiesCommand');
    }

    collectCodeActions(symbol: AZSymbolInformation, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) && (symbol.isALObject()) && (symbol.kind != AZSymbolKind.Interface) && (symbol.selectionRange.start.line == range.start.line)) {
            let action = new vscode.CodeAction("Sort properties", vscode.CodeActionKind.QuickFix);
            action.command = { command: this.name, title: 'Sort properties...' };
            actions.push(action);
        }
    }

    protected async runAsync(range: vscode.Range) {
        //load syntax tree
        let document = vscode.window.activeTextEditor.document;
        let source = document.getText();

        //get document symbols with properties
        let request : ToolsDocumentSymbolsRequest = new ToolsDocumentSymbolsRequest(source, document.uri.fsPath, true);
        let response : ToolsDocumentSymbolsResponse | undefined = await this._toolsExtensionContext.toolsLangServerClient.getALDocumentSymbols(request);
        if ((response) && (response.root)) {
            let rootSymbol = AZSymbolInformation.fromAny(response.root);                    
            rootSymbol.updateTree(true, true);

            //collect PropertyList symbols
            let propertyListSymbolsList: AZSymbolInformation[] = [];
            rootSymbol.collectChildSymbols(AZSymbolKind.PropertyList, true, propertyListSymbolsList);
            if (propertyListSymbolsList.length > 0) {
                
                await vscode.window.activeTextEditor.edit(editBuilder => {
                    for (let i=0; i<propertyListSymbolsList.length; i++) {
                        this.sortChildItems(propertyListSymbolsList[i], AZSymbolKind.Property, editBuilder);
                    }
                });
            }
        }
    }

}