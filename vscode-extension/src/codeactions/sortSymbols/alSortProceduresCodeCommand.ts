import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { ALCodeAction } from '../alCodeAction';

export class ALSortProceduresCodeCommand extends ALCodeAction {
    constructor(context : DevToolsExtensionContext) {
        super(context, "SortProcedures");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document)) {
            let edit: vscode.WorkspaceEdit | undefined = undefined;

            if ((symbol) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line === range.start.line)) {

                let hasMethods = false;
                let hasTriggers = false;

                let isTrigger = symbol.isTrigger();
                let isMethod = symbol.isMethod();

                if ((isTrigger) || (isMethod)) {
                    symbol = symbol.parent;
                }

                if (symbol) {
                    hasTriggers = isTrigger || symbol.hasTriggers();
                    hasMethods = isMethod || symbol.hasMethods();
                    let showSortProcedures = isMethod || symbol.hasMethods();
                    let showSortTriggers = isTrigger || symbol.hasTriggers();

                    if (showSortProcedures) {
                        let caption = showSortTriggers?"Sort procedures and triggers (AZ AL Dev Tools)":"Sort procedures (AZ AL Dev Tools)";
                        let action = new vscode.CodeAction(caption, vscode.CodeActionKind.QuickFix);
                        action.command = {
                            command: "azALDevTools.sortProcedures",
                            title: "Sort Procedures",
                            arguments: [document, symbol.range]
                        };
                        actions.push(action);
                    }

                    if (showSortTriggers) {
                        let action = new vscode.CodeAction("Sort triggers (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                        action.command = {
                            command: "azALDevTools.sortTriggers",
                            title: "Sort Triggers",
                            arguments: [document, symbol.range]
                        };
                        actions.push(action);
                    }
                }
            }

        }
    }

}
