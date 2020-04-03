import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';

export class ALSortReportColumnsCommand extends ALBaseSortCodeCommand {

    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALSortReportColumnsCommand');
    }

    protected async runAsync(range: vscode.Range) {
        // Get required details from document source code
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if (!symbol) {
            return;
        }
        
        let dataItemSymbol: AZSymbolInformation | undefined;
        if (symbol.kind == AZSymbolKind.ReportDataItem)
            dataItemSymbol = symbol;
        else {
            dataItemSymbol = symbol.findParentByKind(AZSymbolKind.ReportDataItem);
            if (!dataItemSymbol)
                return;
        }

        await this.sortDataItemAsync(dataItemSymbol);
    }

    protected async sortDataItemAsync(dataItemSymbol: AZSymbolInformation) {
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
        await vscode.window.activeTextEditor.edit(editBuilder => {
            for (const column of columnsList) {
                const deleteRange = new vscode.Range(column.range.start.line, column.range.start.character, 
                    column.range.end.line, column.range.end.character);
                editBuilder.delete(deleteRange);
            }
            
            editBuilder.insert(insertPos, newSource);
        });
    }


}