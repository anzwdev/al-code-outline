import * as vscode from 'vscode';
import { AZDocumentSymbolsLibrary } from "./azDocumentSymbolsLibrary";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class AZActiveDocumentSymbolsLibrary extends AZDocumentSymbolsLibrary {
    private _autoReload : boolean;
  
    constructor(context : DevToolsExtensionContext) {
        super(context, undefined);
        this._autoReload = true;

        //active document changed - symbols need reloading
		vscode.window.onDidChangeActiveTextEditor(editor => {
            this.onActiveDocumentChanged();
        });
    
        //active document content changed - symbols need update
        vscode.workspace.onDidChangeTextDocument(e => {
            if ((this.isActiveDocument(e.document)) && (e.document.languageId != "al"))
                this.onActiveDocumentChanged();
        });

        //document saved - symbols should be reloaded because document could be formatted
        vscode.workspace.onDidSaveTextDocument(document => {
            if (this.isActiveDocument(document))
                this.onActiveDocumentChanged();
        });
    }

    protected onActiveDocumentChanged() {
        if (vscode.window.activeTextEditor)
            this.setDocUri(vscode.window.activeTextEditor.document.uri);
        else
            this.setDocUri(undefined);
        if (this._autoReload)
            this.loadAsync(false);
    }

    protected onActiveDocumentContentChanged() {
        this._reloadRequired = true;
        if (this._autoReload)
            this.loadAsync(false);
    }

    public initializeContent() {
        this.onActiveDocumentChanged();
    }

}