import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from "../../symbollibraries/azSymbolKind";
import { ALCodeAction } from '../alCodeAction';

export class ALSortVariablesCommand extends ALCodeAction {
    constructor(context : DevToolsExtensionContext) {
        super(context, "SortVariables");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document.uri)) {
            let edit: vscode.WorkspaceEdit | undefined = undefined;

            //collect list of objects in selection range
            if ((symbol) && 
                ((symbol.isALObject()) || (symbol.kind == AZSymbolKind.VarSection) || (symbol.kind == AZSymbolKind.GlobalVarSection)) && 
                (!symbol.containsDiagnostics) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line == range.start.line)) {

                    let action = new vscode.CodeAction("Sort variables (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                    action.command = {
                        command: "azALDevTools.sortVariables",
                        title: "Sort Variables",
                        arguments: [document, symbol.range]
                    }
                    actions.push(action);
                }
        }
    }

}