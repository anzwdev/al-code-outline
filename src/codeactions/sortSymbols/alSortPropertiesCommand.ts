import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { ALCodeAction } from '../alCodeAction';

export class ALSortPropertiesCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortProperties");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document.uri)) {
            let edit: vscode.WorkspaceEdit | undefined =  undefined;

            //collect list of objects in selection range
            if ((symbol) && (symbol.isALObject()) && (symbol.kind != AZSymbolKind.Interface) && 
                (!symbol.containsDiagnostics) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line == range.start.line)) {

                let action = new vscode.CodeAction("Sort properties", vscode.CodeActionKind.QuickFix);
                action.command = {
                    command: "azALDevTools.sortProperties",
                    title: "Sort Properties",
                    arguments: [document, symbol.range]
                }
                actions.push(action);
            }
        }
    }

}