import * as vscode from 'vscode';
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALSortPropertiesCommand extends ALBaseSortCodeCommand {

    constructor(context : DevToolsExtensionContext) {
        super(context);
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) && (symbol.isALObject()) && (symbol.kind != AZSymbolKind.Interface) && (symbol.selectionRange.start.line == range.start.line)) {
            let edit = this.prepareEdit(symbol, document);
            if (edit) {
                let action = new vscode.CodeAction("Sort properties", vscode.CodeActionKind.QuickFix);
                action.edit = edit;
                actions.push(action);
            }
        }
    }

    protected prepareEdit(symbol: AZSymbolInformation, document: vscode.TextDocument): vscode.WorkspaceEdit | undefined {
        //load syntax tree
        //let document = vscode.window.activeTextEditor.document;
        let source = document.getText();

        //collect PropertyList symbols
        let propertyListSymbolsList: AZSymbolInformation[] = [];
        symbol.collectChildSymbols(AZSymbolKind.PropertyList, true, propertyListSymbolsList);
 
        if (propertyListSymbolsList.length == 0)
            return undefined;

        let edit = new vscode.WorkspaceEdit();
        if (propertyListSymbolsList.length > 0) {
            for (let i=0; i<propertyListSymbolsList.length; i++) {
                this.sortChildItems(document.uri, propertyListSymbolsList[i], AZSymbolKind.Property, edit);
            }
        }
        return edit;
    }

}