import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { LSWorkspaceChangeTrackingClient } from '../../langserver/workspacechangetracking/lsWorkspaceChangeTrackingClient';
import { LSWorkspaceFoldersChangeRequest } from '../../langserver/workspacechangetracking/lsWorkspaceFoldersChangeRequest';
import { LSDocumentChangeRequest } from '../../langserver/workspacechangetracking/lsDocumentChangeRequest';
import { LSDocumentContentChangeRequest } from '../../langserver/workspacechangetracking/lsDocumentContentChangeRequest';
import { LSFileSystemFileChangeRequest } from '../../langserver/workspacechangetracking/lsFileSystemFileChangeRequest';
import { LSALProjectSource } from '../../langserver/workspacechangetracking/lsALProjectSource';
import { LSConfigurationChangeRequest } from '../../langserver/workspacechangetracking/lsConfigurationChangeRequest';

export class WorkspaceChangeTrackingService extends DevToolsExtensionService {

    private _lsChangeTrackingClient : LSWorkspaceChangeTrackingClient;
    private _onDidChangeDocumentSymbolsEmiter: vscode.EventEmitter<vscode.Uri | null>;

    readonly onDidChangeDocumentSymbols: vscode.Event<vscode.Uri | null>;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._onDidChangeDocumentSymbolsEmiter = new vscode.EventEmitter<vscode.Uri | null>();
        this.onDidChangeDocumentSymbols = this._onDidChangeDocumentSymbolsEmiter.event;
        
        this._lsChangeTrackingClient = new LSWorkspaceChangeTrackingClient(this.context.lsConnector);

        this.registerEventHandlers();
        this.initializeWorkspace();

    }

    protected registerEventHandlers() {
        //workspace folders events
        this.subscriptions.push(
            vscode.workspace.onDidChangeWorkspaceFolders(e => {
                this._lsChangeTrackingClient.workspaceFolderChange(new LSWorkspaceFoldersChangeRequest(e.added, e.removed));
            }));

        //document events
        this.subscriptions.push(
            vscode.workspace.onDidOpenTextDocument(e => {
                this._lsChangeTrackingClient.documentOpen(new LSDocumentChangeRequest(e.uri.fsPath, undefined));
            }));

        this.subscriptions.push(
            vscode.workspace.onDidChangeTextDocument(async e => {                
                if ((e.document) && (e.document.uri)) {
                    //!!! TODO: bring back symbols update
                    //!!!let buildSymbols = (this._context.activeDocumentSymbols.isActiveDocument(e.document)) && (e.document.languageId == "al");
                    let buildSymbols = false;
                    
                    let response = await this._lsChangeTrackingClient.documentChange(new LSDocumentContentChangeRequest(e.document.uri.fsPath, e.document.getText(), buildSymbols));

                    if (this._onDidChangeDocumentSymbolsEmiter) {
                        this._onDidChangeDocumentSymbolsEmiter.fire(e.document.uri);
                    }
                }
            }));

        this.subscriptions.push(
            vscode.workspace.onDidCloseTextDocument(e => {
                this._lsChangeTrackingClient.documentClose(new LSDocumentChangeRequest(e.uri.fsPath, undefined));
            }));
    
        let watcher =  vscode.workspace.createFileSystemWatcher("**/*"); //we are tracking all file changes to handle folders  .{al,app,json}");
        watcher.onDidChange(e => {
            this._lsChangeTrackingClient.fileSystemFileChange(new LSFileSystemFileChangeRequest(e.fsPath));
            
            //!!!TODO: restore symbols update
            //!!!//notify symbols service that the app file has changed and any files extracted from this file should be refreshed
            //!!!if (e.path.endsWith('.app')) {
            //!!!    this._context.symbolsService.appFileChanged(e);
            //!!!}
        });
        watcher.onDidCreate(e => {
            this._lsChangeTrackingClient.fileSystemFileCreate(new LSFileSystemFileChangeRequest(e.fsPath));
        });
        watcher.onDidDelete(e => {
            this._lsChangeTrackingClient.fileSystemFileDelete(new LSFileSystemFileChangeRequest(e.fsPath));
        });
       
        this.subscriptions.push(watcher);

        this.subscriptions.push(vscode.workspace.onDidChangeConfiguration(configChange => {
            this.onConfigurationChange(configChange);
        }));
    }

    protected initializeWorkspace() {
        this._lsChangeTrackingClient.workspaceFolderChange(
            new LSWorkspaceFoldersChangeRequest(vscode.workspace.workspaceFolders, undefined));
    }

    protected onConfigurationChange(configChange: vscode.ConfigurationChangeEvent) {
        //collect configuration changes        
        let folders = vscode.workspace.workspaceFolders;
        if ((folders) && (folders.length > 0)) {
            let projectSources : LSALProjectSource[] = [];

            for (let i=0; i<folders.length; i++) {
                if ((configChange.affectsConfiguration('al.packageCachePath', folders[i].uri)) || 
                    (configChange.affectsConfiguration('al.codeAnalyzers', folders[i].uri)) ||
                    (configChange.affectsConfiguration('al.rootNamespace', folders[i].uri)) ||
                    (configChange.affectsConfiguration('alOutline.additionalMandatoryAffixesPatterns', folders[i].uri))) {
                    projectSources.push(new LSALProjectSource(folders[i].uri));
                }
            }

            if (projectSources.length > 0) {
                this._lsChangeTrackingClient.configurationChange(new LSConfigurationChangeRequest(projectSources));
            }
        }
    }

}