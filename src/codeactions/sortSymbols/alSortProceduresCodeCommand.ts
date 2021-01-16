import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { ALCodeAction } from '../alCodeAction';

export class ALSortProceduresCodeCommand extends ALCodeAction {
    constructor(context : DevToolsExtensionContext) {
        super(context, "SortProcedures");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document.uri)) {
            let edit: vscode.WorkspaceEdit | undefined = undefined;

            if ((symbol) &&
                ((symbol.isALObject()) || (symbol.isMethod())) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line == range.start.line)) {

                if (symbol.isMethod())
                    symbol = symbol.findParentObject();

                if ((symbol) && (!symbol.containsDiagnostics)) {
                    let action = new vscode.CodeAction("Sort procedures (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                    action.command = {
                        command: "azALDevTools.sortProcedures",
                        title: "Sort Procedures",
                        arguments: [document, symbol.range]
                    }
                    actions.push(action);
                }

            }
        }
    }

}
