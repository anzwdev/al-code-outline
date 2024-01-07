import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALCodeAction } from "../alCodeAction";

export class ALSortPermissionsCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortPermissions");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document)) {
            let edit: vscode.WorkspaceEdit | undefined =  undefined;

            //collect list of objects in selection range
            if ((symbol) && 
                (symbol.kind === AZSymbolKind.Property) && 
                (symbol.name) && 
                (symbol.name.toLowerCase() === "permissions") &&
                (!symbol.containsDiagnostics) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line === range.start.line)) {

                let action = new vscode.CodeAction("Sort permissions (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                action.command = {
                    command: "azALDevTools.sortPermissions",
                    title: "Sort Permissions",
                    arguments: [document, symbol.range]
                };
                actions.push(action);
            }
        }
    }

}