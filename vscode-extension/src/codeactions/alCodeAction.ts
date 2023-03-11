import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../symbollibraries/azSymbolInformation";
import { ALCodeActionsProvider } from './alCodeActionsProvider';

export class ALCodeAction {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _shortName: string;
    
    constructor(context : DevToolsExtensionContext, shortName: string) {
        this._toolsExtensionContext = context;
        this._shortName = shortName;
    }    

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
    }

    protected getDocumentUri() : vscode.Uri | undefined {
        return this._toolsExtensionContext.activeDocumentSymbols.getDocUri();
    }

    protected canRunOnSave(document: vscode.TextDocument): boolean {
        if (!this._shortName)
            return false;    

        let configuration = vscode.workspace.getConfiguration('alOutline', document.uri);
        if (!ALCodeActionsProvider.canRunOnSaveOnFile(configuration, document))
            return false;
            
        let actionsList = configuration.get<string[]>('codeActionsOnSave');
        if (actionsList)
            return (actionsList.indexOf(this._shortName) >= 0);
        return false;
    }

}