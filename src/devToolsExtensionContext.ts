'use strict';

import * as vscode from 'vscode';
import { ALLangServerProxy } from './allanguage/alLangServerProxy';
import { ToolsLangServerClient } from './langserver/toolsLangServerClient';
import { AZActiveDocumentSymbolsLibrary } from './symbollibraries/azActiveDocumentSymbolsLibrary';
import { ALObjectRunner } from './alObjectRunner';
import { AZSymbolsLibrary } from './symbollibraries/azSymbolsLibrary';
import { ALSymbolsBrowser } from './alsymbolsbrowser/alSymbolsBrowser';
import { ALOutlineService } from './services/alOutlineService';
import { ALObjectWizardsService } from './services/alObjectWizardsService';
import { ALCompletionService } from './services/alCompletionService';
import { ALSymbolsTreeService } from './services/alSymbolsTreeService';
import { CodeAnalyzersService } from './services/codeAnalyzersService';
import { ALSymbolsService } from './services/alSymbolsService';
import { ALCodeTransformationService } from './services/alCodeTransformationService';
import { ALCodeActionsService } from './services/ALCodeActionsService';
import { EditorsService } from './services/editorsService';
import { WorkspaceChangeTrackingService } from './services/workspaceChangeTrackingService';

export class DevToolsExtensionContext implements vscode.Disposable {
    alLangProxy : ALLangServerProxy;    
    vscodeExtensionContext : vscode.ExtensionContext;
    toolsLangServerClient : ToolsLangServerClient;
    activeDocumentSymbols : AZActiveDocumentSymbolsLibrary;
    objectRunner : ALObjectRunner;
    alOutlineService : ALOutlineService;
    alSymbolsTreeService : ALSymbolsTreeService;
    alWizardsService : ALObjectWizardsService;
    alCompletionService : ALCompletionService;
    codeAnalyzersService: CodeAnalyzersService;
    symbolsService: ALSymbolsService;
    alCodeTransformationService: ALCodeTransformationService;
    alCodeActionsService: ALCodeActionsService;
    editorsService: EditorsService;
    workspaceChangeTrackingService: WorkspaceChangeTrackingService;

    constructor(context : vscode.ExtensionContext) {
        this.alLangProxy = new ALLangServerProxy()
        this.vscodeExtensionContext = context;

        let alExtensionPath : string = "";
        if (this.alLangProxy.extensionPath)
            alExtensionPath = this.alLangProxy.extensionPath;

        this.toolsLangServerClient = new ToolsLangServerClient(context, alExtensionPath, this.alLangProxy.version);
        this.activeDocumentSymbols = new AZActiveDocumentSymbolsLibrary(this);
        this.objectRunner = new ALObjectRunner(this);

        this.alOutlineService = new ALOutlineService(this);
        this.alSymbolsTreeService = new ALSymbolsTreeService(this);
        this.alWizardsService = new ALObjectWizardsService(this);
        this.alCompletionService = new ALCompletionService(this);
        this.codeAnalyzersService = new CodeAnalyzersService(this);
        this.symbolsService = new ALSymbolsService(this);
        this.alCodeTransformationService = new ALCodeTransformationService(this);
        this.alCodeActionsService = new ALCodeActionsService(this);
        this.editorsService = new EditorsService(this);
        this.workspaceChangeTrackingService = new WorkspaceChangeTrackingService(this);
    }

    getUseSymbolsBrowser() : boolean {
        let useSymbolsBrowser = this.vscodeExtensionContext.globalState.get<boolean>("azALDevTools.useSymbolsBrowser");
        if (useSymbolsBrowser)
            return useSymbolsBrowser;
        return false;
    }

    setUseSymbolsBrowser(newValue : boolean) {
        this.vscodeExtensionContext.globalState.update("azALDevTools.useSymbolsBrowser", newValue);
    }

    showSymbolsBrowser(library: AZSymbolsLibrary) {
        let symbolsBrowser : ALSymbolsBrowser = new ALSymbolsBrowser(this, library);
        symbolsBrowser.show();
    }

    dispose() {
    }

}