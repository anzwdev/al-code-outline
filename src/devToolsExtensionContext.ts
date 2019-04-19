'use strict';

import * as vscode from 'vscode';
import { ALLangServerProxy } from './allanguage/alLangServerProxy';
import { ToolsLangServerClient } from './langserver/toolsLangServerClient';
import { AZActiveDocumentSymbolsLibrary } from './symbollibraries/azActiveDocumentSymbolsLibrary';

export class DevToolsExtensionContext implements vscode.Disposable {
    alLangProxy : ALLangServerProxy;    
    vscodeExtensionContext : vscode.ExtensionContext;
    toolsLangServerClient : ToolsLangServerClient;
    activeDocumentSymbols : AZActiveDocumentSymbolsLibrary;

    constructor(context : vscode.ExtensionContext) {
        this.alLangProxy = new ALLangServerProxy()
        this.vscodeExtensionContext = context;

        let alExtensionPath : string = "";
        if (this.alLangProxy.extensionPath)
            alExtensionPath = this.alLangProxy.extensionPath;

        this.toolsLangServerClient = new ToolsLangServerClient(context, alExtensionPath);
        this.activeDocumentSymbols = new AZActiveDocumentSymbolsLibrary(this);    
    }

    getUseSymbolsBrowser() : boolean {
        let useSymbolsBrowser = this.vscodeExtensionContext.globalState.get<boolean>("azALDevTools.useSymbolsBrowser");
        return useSymbolsBrowser;
    }

    setUseSymbolsBrowser(newValue : boolean) {
        this.vscodeExtensionContext.globalState.update("azALDevTools.useSymbolsBrowser", newValue);
    }

    dispose() {
    }

}