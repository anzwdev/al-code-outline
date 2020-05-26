import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALSortReportColumnsCommand extends ALBaseSortCodeCommand {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortReportColumns");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        let edit: vscode.WorkspaceEdit | undefined = undefined;
        let fixOnSave = this.canRunOnSave(document.uri); 
        let actionKind = this.getCodeActionKind(fixOnSave);

        if (fixOnSave) {
            let objList: AZSymbolInformation[] = [];
            if (docSymbols.rootSymbol) {
                docSymbols.rootSymbol.collectObjectSymbols(objList);
                for (let i=0; i<objList.length; i++)
                    edit = this.prepareEdit(objList[i], document, edit);
            }
        } else {
            //prepare code fix
            if ((symbol) &&
                ((symbol.kind == AZSymbolKind.ReportDataItem) ||
                (symbol.kind == AZSymbolKind.ReportColumn) ||
                ((symbol.kind == AZSymbolKind.ReportObject) && (symbol.selectionRange) && (symbol.selectionRange.start.line == range.start.line))))
                edit = this.prepareEdit(symbol, document, edit);
        }

        if (edit) {
            let action = new vscode.CodeAction("Sort data item columns", actionKind);
            action.edit = edit; 
            actions.push(action);
        }
    }

    protected prepareEdit(symbol: AZSymbolInformation, document: vscode.TextDocument, edit: vscode.WorkspaceEdit | undefined): vscode.WorkspaceEdit {
        if (!edit)
            edit = new vscode.WorkspaceEdit();
        switch (symbol.kind) {
            case AZSymbolKind.ReportDataItem:
                this.sortChildItems(document.uri, symbol, AZSymbolKind.ReportColumn, edit);
                break;
            case AZSymbolKind.ReportColumn:
                let dataItemSymbol = symbol.findParentByKind(AZSymbolKind.ReportDataItem);
                if (dataItemSymbol)
                    this.sortChildItems(document.uri, dataItemSymbol, AZSymbolKind.ReportColumn, edit);
                break;
            case AZSymbolKind.ReportObject:
                let dataItemSymbolsList: AZSymbolInformation[] = [];
                symbol.collectChildSymbols(AZSymbolKind.ReportDataItem, true, dataItemSymbolsList);
                if (dataItemSymbolsList.length > 0)
                    for (let i=0; i<dataItemSymbolsList.length; i++) {
                        this.sortChildItems(document.uri, dataItemSymbolsList[i], AZSymbolKind.ReportColumn, edit);
                    }
                break;
        }
        return edit;
    }

}