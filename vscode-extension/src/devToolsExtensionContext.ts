import * as vscode from 'vscode';
import * as path from 'path';
import { LSConnector } from './langserver/lsConnector';
import { WorkspaceChangeTrackingService } from './extension_modules/workspace_change_tracking/workspaceChangeTrackingService';
import { SyntaxTreeSymbolsService } from './extension_modules/syntax_tree_symbols/syntaxTreeSymbolsService';
import { CodeOutlineViewService } from './extension_modules/code_outline_view/codeOutlineViewService';
import { SymbolsViewerService } from './extension_modules/symbols_viewer/symbolsViewerService';
import { MSALLanguageServerProxyService } from './extension_modules/ms_al_langserver/msalLanguageServerProxyService';
import { IdReservationService } from './extension_modules/id_reservations/isReservationService';
import { ProjectInformationService } from './extension_modules/project_information/projectInformationService';
import { CrsIntegrationService } from './extension_modules/crs_integration/crsIntegrationService';
import { ALObjectWizardsService } from './extension_modules/object_wizards/alObjectWizardsService';
import { AppFileTextContentProviderService } from './extension_modules/app_symbol_source_provider/appFileTextContentProviderService';
import { CodeOutlineWebViewService } from './extension_modules/code_outline_webview/codeOutlineWebViewService';
import { ALRawSyntaxTreeViewerService } from './extension_modules/raw_syntax_tree_viewer/alRawSyntaxTreeViewerService';

export class DevToolsExtensionContext implements vscode.Disposable {
    vscodeExtensionContext : vscode.ExtensionContext;
    lsConnector : LSConnector;
    subscriptions: vscode.Disposable[] = [];

    public msalLanguageServerProxyService : MSALLanguageServerProxyService;
    public changeTrackingService : WorkspaceChangeTrackingService;
    public idReservationService : IdReservationService;
    public projectInformationService : ProjectInformationService;
    public appFileTextContentProviderService : AppFileTextContentProviderService;

    public syntaxTreeSymbolsService : SyntaxTreeSymbolsService;
    public codeOutlineViewService : CodeOutlineViewService;
    public symbolsViewerService : SymbolsViewerService;
    public codeOutlineWebViewService : CodeOutlineWebViewService;
    public alRawSyntaxTreeViewerService : ALRawSyntaxTreeViewerService;

    public alObjectWizardsService : ALObjectWizardsService;

    public crsIntegrationService : CrsIntegrationService;

    constructor(context : vscode.ExtensionContext) {
        this.vscodeExtensionContext = context;
        
        // Initialize language server connector and services
        this.lsConnector = new LSConnector(context);        

        this.msalLanguageServerProxyService = new MSALLanguageServerProxyService(this);
        this.changeTrackingService = new WorkspaceChangeTrackingService(this);
        this.idReservationService = new IdReservationService(this);
        this.projectInformationService = new ProjectInformationService(this);
        this.appFileTextContentProviderService = new AppFileTextContentProviderService(this);

        this.syntaxTreeSymbolsService = new SyntaxTreeSymbolsService(this);
        this.codeOutlineViewService = new CodeOutlineViewService(this);
        this.symbolsViewerService = new SymbolsViewerService(this);
        this.codeOutlineWebViewService = new CodeOutlineWebViewService(this);
        this.alRawSyntaxTreeViewerService = new ALRawSyntaxTreeViewerService(this);

        this.alObjectWizardsService = new ALObjectWizardsService(this);

        this.crsIntegrationService = new CrsIntegrationService(this);

        // Add disposables to subscriptions
        this.subscriptions.push(this.lsConnector);

        this.subscriptions.push(this.msalLanguageServerProxyService);
        this.subscriptions.push(this.changeTrackingService);
        this.subscriptions.push(this.idReservationService);
        this.subscriptions.push(this.projectInformationService);
        this.subscriptions.push(this.appFileTextContentProviderService);
        
        this.subscriptions.push(this.syntaxTreeSymbolsService);
        this.subscriptions.push(this.codeOutlineViewService);
        this.subscriptions.push(this.symbolsViewerService);
        this.subscriptions.push(this.codeOutlineWebViewService);
        this.subscriptions.push(this.alRawSyntaxTreeViewerService);

        this.subscriptions.push(this.alObjectWizardsService);
        
        this.subscriptions.push(this.crsIntegrationService);
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

    getGlobalStateSetting<T>(key: string, defaultValue: T) : T {
        let value = this.vscodeExtensionContext.globalState.get<T>(key);
        return value !== undefined ? value : defaultValue;
    }

    setGlobalStateSetting<T>(key: string, value: T) {
        this.vscodeExtensionContext.globalState.update(key, value);
    }

    dispose() {
        for (let i = 0; i < this.subscriptions.length; i++) {
            this.subscriptions[i].dispose();
        }
        this.subscriptions = [];
    }

}