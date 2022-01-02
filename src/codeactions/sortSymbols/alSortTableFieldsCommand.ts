import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALCodeAction } from "../alCodeAction";

export class ALSortTableFieldsCommand extends ALCodeAction {
    
    constructor(context : DevToolsExtensionContext) {
        super(context, "SortTableFields");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document)) {
            let edit: vscode.WorkspaceEdit | undefined = undefined;

            if ((symbol) &&
                (
                    (symbol.kind == AZSymbolKind.TableObject) || 
                    (symbol.kind == AZSymbolKind.TableExtensionObject) || 
                    (symbol.kind == AZSymbolKind.Field) ||
                    (symbol.kind == AZSymbolKind.FieldExtensionList) ||
                    (symbol.kind == AZSymbolKind.FieldList)) &&
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line == range.start.line)) {

                if ((symbol.kind == AZSymbolKind.Field) ||
                    (symbol.kind == AZSymbolKind.FieldExtensionList) ||
                    (symbol.kind == AZSymbolKind.FieldList))
                    symbol = symbol.findParentObject();

                if ((symbol) && (!symbol.containsDiagnostics)) {
                    let action = new vscode.CodeAction("Sort fields (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                    action.command = {
                        command: "azALDevTools.sortTableFields",
                        title: "Sort Fields",
                        arguments: [document, symbol.range]
                    }
                    actions.push(action);
                }

            }
        }
    }

}