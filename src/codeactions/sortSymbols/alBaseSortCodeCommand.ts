import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from "../../symbollibraries/azSymbolKind";
import { ALCodeAction } from '../alCodeAction';

export class ALBaseSortCodeCommand extends ALCodeAction {
    constructor(context : DevToolsExtensionContext, shortName: string) {
        super(context, shortName);
    }

    protected sortChildItems(docUri: vscode.Uri, symbol: AZSymbolInformation, childSymbolKind: AZSymbolKind, editBuilder: vscode.WorkspaceEdit) {
        // Collect columns
        let childSymbolsList: AZSymbolInformation[] = [];
        symbol.collectChildSymbols(childSymbolKind, false, childSymbolsList);
        if (childSymbolsList.length == 0) {
            return;
        }

        let insertPos: vscode.Position = new vscode.Position(childSymbolsList[0].range.start.line, childSymbolsList[0].range.start.character);

        // Sort columns
        childSymbolsList.sort((symbolA, symbolB) => {
            return this.compareSymbols(symbolA, symbolB);
        });

        // Produce the new sorted source
        let newSource: string = "";
        for (const childSymbol of childSymbolsList) {
            const declRange = new vscode.Range(childSymbol.range.start.line, childSymbol.range.start.character, 
                childSymbol.range.end.line, childSymbol.range.end.character);
            newSource += vscode.window.activeTextEditor.document.getText(declRange);
        }
        
        // Delete the old unsorted columns and insert the new sorted source
        for (const childSymbol of childSymbolsList) {
            const deleteRange = new vscode.Range(childSymbol.range.start.line, childSymbol.range.start.character, 
                childSymbol.range.end.line, childSymbol.range.end.character);
            editBuilder.delete(docUri, deleteRange);
        }
        
        editBuilder.insert(docUri, insertPos, newSource);
    }

    protected compareSymbols(symbolA: AZSymbolInformation, symbolB: AZSymbolInformation): number {
        return symbolA.name.localeCompare(symbolB.name, undefined, { numeric: true, sensitivity: 'base' });
    }

}
