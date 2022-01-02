import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { TextRange } from '../symbollibraries/textRange';
import { ALCodeAction } from "./alCodeAction";

export class ALReuseToolTipCodeCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SuggestToolTip");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        let edit: vscode.WorkspaceEdit | undefined =  undefined;

        //collect list of objects in selection range
        if ((symbol) &&
            (symbol.selectionRange) &&
            (symbol.selectionRange.start.line == range.start.line)) {

            let fieldSymbol = symbol;
            if (fieldSymbol.kind != AZSymbolKind.PageField) {
                if (fieldSymbol.kind == AZSymbolKind.Property)
                    if ((fieldSymbol.parent) && (fieldSymbol.parent.parent) && (fieldSymbol.parent.parent.kind == AZSymbolKind.PageField))
                        fieldSymbol = fieldSymbol.parent.parent;
            }
            if ((fieldSymbol.kind == AZSymbolKind.PageField) &&
                (fieldSymbol.source) &&
                (fieldSymbol.source != '') &&
                (!symbol.containsDiagnostics)) {
             
                
                let objectSymbol = symbol.findParentObject();
                if ((objectSymbol) &&
                    ((objectSymbol.kind == AZSymbolKind.PageObject) || (objectSymbol.kind == AZSymbolKind.PageExtensionObject))) {
               
                    let action = new vscode.CodeAction("Reuse ToolTip from other Pages (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                    action.command = {
                        command: "azALDevTools.ReuseToolTipFromOtherPages",
                        title: "Reuse tooltip from other pages",
                        arguments: [document, symbol]
                    }
                    actions.push(action);
                }

            }
        }
    }

}