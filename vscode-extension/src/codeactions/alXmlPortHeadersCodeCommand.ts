import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from "../symbollibraries/azSymbolKind";
import { ALCodeAction } from "./alCodeAction";

export class ALXmlPortHeadersCodeCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "XmlPortHeadersActions");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        if ((symbol) &&
            ((symbol.kind == AZSymbolKind.XmlPortTableElement) || (symbol.kind == AZSymbolKind.XmlPortObject))
        ) {
            let appObject = symbol.findParentObject();
            if ((appObject) && (appObject.format == "variabletext")) {
                let usedObjectsAction = new vscode.CodeAction("Generate column headers for CSV export (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                usedObjectsAction.command = {
                    command: "azALDevTools.generateCSVXmlPortHeaders",
                    title: "Generate CSV XmlPort Headers",
                    arguments: [document, symbol.range]
                }
                actions.push(usedObjectsAction);
            }
        }
    }

}
