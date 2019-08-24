'use strict';

import * as vscode from 'vscode';
import { ALLangServerProxy } from './allanguage/alLangServerProxy';
import { ToolsLangServerClient } from './langserver/toolsLangServerClient';
import { AZActiveDocumentSymbolsLibrary } from './symbollibraries/azActiveDocumentSymbolsLibrary';
import { ALObjectRunner } from './alObjectRunner';
import { AZSymbolsLibrary } from './symbollibraries/azSymbolsLibrary';
import { ALSymbolsBrowser } from './alsymbolsbrowser/alSymbolsBrowser';
import { ALObjectsBrowser } from './alsymbolsbrowser/alObjectsBrowser';
import { ALOutlineService } from './services/alOutlineService';
import { ALObjectWizardsService } from './services/alObjectWizardsService';
import { ALCompletionService } from './services/alCompletionService';

export class DevToolsExtensionContext implements vscode.Disposable {
    alLangProxy : ALLangServerProxy;    
    vscodeExtensionContext : vscode.ExtensionContext;
    toolsLangServerClient : ToolsLangServerClient;
    activeDocumentSymbols : AZActiveDocumentSymbolsLibrary;
    objectRunner : ALObjectRunner;
    alOutlineService : ALOutlineService;
    alWizardsService : ALObjectWizardsService;
    alCompletionService : ALCompletionService;

    constructor(context : vscode.ExtensionContext) {
        this.alLangProxy = new ALLangServerProxy()
        this.vscodeExtensionContext = context;

        let alExtensionPath : string = "";
        if (this.alLangProxy.extensionPath)
            alExtensionPath = this.alLangProxy.extensionPath;

        this.toolsLangServerClient = new ToolsLangServerClient(context, alExtensionPath);
        this.activeDocumentSymbols = new AZActiveDocumentSymbolsLibrary(this);
        this.objectRunner = new ALObjectRunner(this);

        this.alOutlineService = new ALOutlineService(this);
        this.alWizardsService = new ALObjectWizardsService(this);
        this.alCompletionService = new ALCompletionService(this);
    }

    getUseSymbolsBrowser() : boolean {
        let useSymbolsBrowser = this.vscodeExtensionContext.globalState.get<boolean>("azALDevTools.useSymbolsBrowser");
        return useSymbolsBrowser;
    }

    setUseSymbolsBrowser(newValue : boolean) {
        this.vscodeExtensionContext.globalState.update("azALDevTools.useSymbolsBrowser", newValue);
    }

    showSymbolsBrowser(library: AZSymbolsLibrary) {
        if (this.getUseSymbolsBrowser()) {
            let symbolsBrowser : ALSymbolsBrowser = new ALSymbolsBrowser(this, library);
            symbolsBrowser.show();
        } else {
            let objectsBrowser : ALObjectsBrowser = new ALObjectsBrowser(this, library);
            objectsBrowser.show();
        }        

    }

    dispose() {
    }

}