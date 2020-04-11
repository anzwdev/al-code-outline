import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";

export class ALCodeAction {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    
    constructor(context : DevToolsExtensionContext) {
        this._toolsExtensionContext = context;
    }    

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
    }

    protected getDocumentUri() : vscode.Uri {
        return this._toolsExtensionContext.activeDocumentSymbols.getDocUri();
    }

    protected canRunOnSave(actionCode: string, resource?: vscode.Uri) {
        let actionsList = vscode.workspace.getConfiguration('alOutline', resource).get<string[]>('codeActionsOnSave');
        return (actionsList.indexOf(actionCode) >= 0);
    }

}