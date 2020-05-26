import * as vscode from 'vscode';
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALSortPropertiesCommand extends ALBaseSortCodeCommand {

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortProperties");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        let edit: vscode.WorkspaceEdit | undefined =  undefined;
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
            //collect list of objects in selection range
            if ((symbol) && (symbol.isALObject()) && (symbol.kind != AZSymbolKind.Interface) && 
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line == range.start.line))                
                edit = this.prepareEdit(symbol, document, undefined);
        }

        if (edit) {
            let action = new vscode.CodeAction("Sort properties", actionKind);
            action.edit = edit;
            actions.push(action);
        }
    }

    protected prepareEdit(symbol: AZSymbolInformation, document: vscode.TextDocument, edit: vscode.WorkspaceEdit | undefined): vscode.WorkspaceEdit | undefined {
        //collect PropertyList symbols
        let propertyListSymbolsList: AZSymbolInformation[] = [];
        symbol.collectChildSymbols(AZSymbolKind.PropertyList, true, propertyListSymbolsList);
 
        if (propertyListSymbolsList.length == 0)
            return edit;

        if (!edit)
            edit = new vscode.WorkspaceEdit();
        if (propertyListSymbolsList.length > 0) {
            for (let i=0; i<propertyListSymbolsList.length; i++) {
                this.sortChildItems(document.uri, propertyListSymbolsList[i], AZSymbolKind.Property, edit);
            }
        }
        return edit;
    }

}