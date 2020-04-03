import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';

export class ALSortReportColumnsCommand extends ALBaseSortCodeCommand {

    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALSortReportColumnsCommand');
    }

    collectCodeActions(symbol: AZSymbolInformation, range: vscode.Range | vscode.Selection, actions: vscode.CodeAction[]) {
        if ((symbol.kind == AZSymbolKind.ReportDataItem) ||
            (symbol.kind == AZSymbolKind.ReportColumn) ||
            ((symbol.kind == AZSymbolKind.ReportObject) && (symbol.selectionRange.start.line == range.start.line))) {
            let action = new vscode.CodeAction("Sort data item columns", vscode.CodeActionKind.QuickFix);
            action.command = { command: this.name, title: 'Sort data item columns...' };
            actions.push(action);
        }
    }

    protected async runAsync(range: vscode.Range) {
        // Get required details from document source code
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if (!symbol) {
            return;
        }
        
        switch (symbol.kind) {
            case AZSymbolKind.ReportDataItem:
                await this.sortDataItemAsync(symbol);
                break;
            case AZSymbolKind.ReportColumn:
                let dataItemSymbol = symbol.findParentByKind(AZSymbolKind.ReportDataItem);
                if (dataItemSymbol)
                    await this.sortDataItemAsync(symbol);
                break;
            case AZSymbolKind.ReportObject:
                let dataItemSymbolsList: AZSymbolInformation[] = [];
                symbol.collectChildSymbols(AZSymbolKind.ReportDataItem, true, dataItemSymbolsList);
                if (dataItemSymbolsList.length > 0)
                    await this.sortDataItemsListAsync(dataItemSymbolsList);
                break;
        }
    }

    protected async sortDataItemsListAsync(dataItemSymbolsList: AZSymbolInformation[]) {
        await vscode.window.activeTextEditor.edit(editBuilder => {
            for (let i=0; i<dataItemSymbolsList.length; i++) {
                this.sortChildItems(dataItemSymbolsList[i], AZSymbolKind.ReportColumn, editBuilder);
            }
        });
    }

    protected async sortDataItemAsync(dataItemSymbol: AZSymbolInformation) {
        await vscode.window.activeTextEditor.edit(editBuilder => {
            this.sortChildItems(dataItemSymbol, AZSymbolKind.ReportColumn, editBuilder);
        });
    }

}