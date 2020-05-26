import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";

export class ALCodeAction {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _shortName: string;
    
    constructor(context : DevToolsExtensionContext, shortName: string) {
        this._toolsExtensionContext = context;
        this._shortName = shortName;
    }    

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
    }

    protected getDocumentUri() : vscode.Uri | undefined {
        return this._toolsExtensionContext.activeDocumentSymbols.getDocUri();
    }

    protected canRunOnSave(resource?: vscode.Uri): boolean {
        if (!this._shortName)
            return false;    
            
        let actionsList = vscode.workspace.getConfiguration('alOutline', resource).get<string[]>('codeActionsOnSave');
        if (actionsList)
            return (actionsList.indexOf(this._shortName) >= 0);
        return false;
    }

    protected getCodeActionKind(fixOnSave: boolean): vscode.CodeActionKind {
        if (fixOnSave)
            return vscode.CodeActionKind.SourceFixAll; //.append('al');
        return vscode.CodeActionKind.QuickFix;
    }

}