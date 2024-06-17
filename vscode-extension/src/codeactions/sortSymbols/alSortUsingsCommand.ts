import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALCodeAction } from "../alCodeAction";

export class ALSortUsingsCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortUsings");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document)) {
            let edit: vscode.WorkspaceEdit | undefined =  undefined;

            //collect list of objects in selection range
            if ((symbol) && 
                (symbol.kind === AZSymbolKind.UsingDirective) && 
                //Disabled diagnostics check because it might contain warning that using is not used
                //which would block the command
                //(!symbol.containsDiagnostics) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line === range.start.line)) {

                let action = new vscode.CodeAction("Sort usings (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                action.command = {
                    command: "azALDevTools.sortUsings",
                    title: "Sort Usings",
                    arguments: [document, symbol.range]
                };
                actions.push(action);
            }
        }
    }

}