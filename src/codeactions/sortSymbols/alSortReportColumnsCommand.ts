import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { ALCodeAction } from '../alCodeAction';

export class ALSortReportColumnsCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortReportColumns");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if (!this.canRunOnSave(document.uri)) {
            let edit: vscode.WorkspaceEdit | undefined = undefined;

            //prepare code fix
            if ((symbol) &&
                ((symbol.kind == AZSymbolKind.ReportDataItem) ||
                 (symbol.kind == AZSymbolKind.ReportColumn) ||
                 (symbol.kind == AZSymbolKind.ReportObject)) &&                
                (symbol.selectionRange) && 
                (symbol.selectionRange.start.line == range.start.line)) {

                //find parent symbol for report column
                if (symbol.kind == AZSymbolKind.ReportColumn)
                    symbol = symbol.findParentByKind(AZSymbolKind.ReportDataItem);
    
                if ((symbol) && (!symbol.containsDiagnostics)) {
                    let action = new vscode.CodeAction("Sort data item columns", vscode.CodeActionKind.QuickFix);
                    action.command = {
                        command: "azALDevTools.sortReportColumns",
                        title: "Sort Report Columns",
                        arguments: [document, symbol.range]
                    }
                    actions.push(action);
                }
            }
        }
    }
}