'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
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
import { DiagnosticsService } from './services/diagnosticsService';
import { ALImagesService } from './services/alImagesService';
import { DuplicateCodeService } from './services/duplicateCodeService';
import { WarningDirectivesService } from './services/warningDirectivesService';
import { HoverService } from './services/hoverService';
import { ReferencesService } from './services/referencesService';
import { IdReservationService } from './services/idReservationService';
import { ALDecorationService } from './services/alDecorationService';

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
    diagnosticsService: DiagnosticsService;
    alImagesService: ALImagesService;
    duplicateCodeService: DuplicateCodeService;
    warningDirectivesService: WarningDirectivesService;
    hoverService: HoverService;
    referencesService: ReferencesService;
    idReservationService: IdReservationService;
    alDecorationService: ALDecorationService;

    constructor(context : vscode.ExtensionContext) {
        this.alLangProxy = new ALLangServerProxy();
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
        this.diagnosticsService = new DiagnosticsService(this);
        this.alImagesService = new ALImagesService(this);
        this.duplicateCodeService = new DuplicateCodeService(this);
        this.warningDirectivesService = new WarningDirectivesService(this);
        this.hoverService = new HoverService(this);
        this.referencesService = new ReferencesService(this);
        this.idReservationService = new IdReservationService(this);
        this.alDecorationService = new ALDecorationService(this);
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

    getImageUri(name: string, theme: string) {
        return vscode.Uri.file(this.vscodeExtensionContext.asAbsolutePath(path.join("resources", "images", theme, name)));
    }

    getLightImageUri(name: string) {
        return this.getImageUri(name, "light");
    }

    getDarkImageUri(name: string) {
        return this.getImageUri(name, "dark");
    }

    dispose() {
        this.toolsLangServerClient.dispose();
    }

}