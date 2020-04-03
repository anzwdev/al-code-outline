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
                this.sortDataItem(dataItemSymbolsList[i], editBuilder);
            }
        });
    }

    protected async sortDataItemAsync(dataItemSymbol: AZSymbolInformation) {
        await vscode.window.activeTextEditor.edit(editBuilder => {
            this.sortDataItem(dataItemSymbol, editBuilder);
        });
    }

    protected sortDataItem(dataItemSymbol: AZSymbolInformation, editBuilder: vscode.TextEditorEdit) {
        // Collect columns
        let columnsList: AZSymbolInformation[] = [];
        dataItemSymbol.collectChildSymbols(AZSymbolKind.ReportColumn, false, columnsList);
        if (columnsList.length == 0) {
            return;
        }

        let insertPos: vscode.Position = new vscode.Position(columnsList[0].range.start.line, columnsList[0].range.start.character);

        // Sort columns
        columnsList.sort((columnA, columnB) => {
            return columnA.name.localeCompare(columnB.name, undefined, { numeric: true, sensitivity: 'base' });
        });

        // Produce the new sorted source
        let newSource: string = "";
        for (const column of columnsList) {
            const declRange = new vscode.Range(column.range.start.line, column.range.start.character, 
                column.range.end.line, column.range.end.character);
            newSource += vscode.window.activeTextEditor.document.getText(declRange);
        }
        
        // Delete the old unsorted columns and insert the new sorted source
        for (const column of columnsList) {
            const deleteRange = new vscode.Range(column.range.start.line, column.range.start.character, 
                column.range.end.line, column.range.end.character);
            editBuilder.delete(deleteRange);
        }
        
        editBuilder.insert(insertPos, newSource);
    }


}