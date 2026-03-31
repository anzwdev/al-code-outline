import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { SymbolsViewerConst } from './symbolsViewerConst';
import { LSSymbolsViewerClient } from '../../langserver/symbols_viewer/lsSymbolsViewerClient';
import { ProjectSymbolsViewerLoader } from './projectSymbolsViewerLoader';
import { AppFileSymbolsViewerLoader } from './appFileSymbolsViewerLoader';
import { ALSymbolsBrowser } from './alSymbolsBrowser';
import { AppFileSymbolsViewerProvider } from './appFileSymbolsViewerProvider';

export class SymbolsViewerService extends DevToolsExtensionService {
    private _lssymbolsViewerClient: LSSymbolsViewerClient;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._lssymbolsViewerClient = new LSSymbolsViewerClient(context.lsConnector);

        this.registerCommands();
        this.registerEditors();
    }

    private registerCommands() {
        //al app viewer
        this.subscriptions.push(
            vscode.commands.registerCommand(
                SymbolsViewerConst.cmdViewALApp,
                (fileUri) => {
                    this.showAppFileSymbolsViewer(fileUri?.fsPath);
                }));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                SymbolsViewerConst.cmdShowAllProjectSymbols,
                () => {
                    this.showProjectSymbolsViewer(true);
                }
            )
        );

        this.subscriptions.push(
            vscode.commands.registerCommand(
                SymbolsViewerConst.cmdShowProjectSymbolsWithoutDep,
                () => {
                    this.showProjectSymbolsViewer(false);
                }
            )
        );
    }

    private registerEditors() {
        this.subscriptions.push(
            vscode.window.registerCustomEditorProvider(SymbolsViewerConst.appFileSymbolsViewerViewType,
                new AppFileSymbolsViewerProvider(this.context), { webviewOptions: { retainContextWhenHidden: true }}));
    }

    async createAppFileSymbolsViewer(filePath: string) : Promise<ALSymbolsBrowser | undefined> {
        let loader = new AppFileSymbolsViewerLoader(this._lssymbolsViewerClient, filePath);
        let browser = new ALSymbolsBrowser(this.context, loader, SymbolsViewerConst.appFileSymbolsViewerViewType);
        return browser;
    }

    async showAppFileSymbolsViewer(filePath: string) {
        let browser = await this.createAppFileSymbolsViewer(filePath);
        if (browser) {
            browser.show();
        }
    }

    async attachAppFileSymbolsViewer(filePath: string, webviewPanel: vscode.WebviewPanel) {
        let browser = await this.createAppFileSymbolsViewer(filePath);
        if (browser) {
            browser.attachToWebView(webviewPanel);
        }
    }

    async showProjectSymbolsViewer(includeDependencies: boolean) {
        let workspaceUri = this.context.msalLanguageServerProxyService.getCurrentALWorkspaceUri();
        if (workspaceUri?.fsPath) {
            let loader = new ProjectSymbolsViewerLoader(this._lssymbolsViewerClient, workspaceUri.fsPath, includeDependencies);
            let browser = new ALSymbolsBrowser(this.context, loader, SymbolsViewerConst.projectSymbolsViewerViewType);
            browser.show();
        }
    }

}