import { config } from 'process';
import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsALProjectSource } from '../langserver/toolsALProjectSource';
import { ToolsConfigurationChangeRequest } from '../langserver/toolsConfigurationChangeRequest';
import { ToolsDocumentChangeRequest } from '../langserver/toolsDocumentChangeRequest';
import { ToolsDocumentContentChangeRequest } from '../langserver/toolsDocumentContentChangeRequest';
import { ToolsFileSystemFileChangeRequest } from '../langserver/toolsFileSystemFileChangeRequest';
import { ToolsWorkspaceFoldersChangeRequest } from '../langserver/toolsWorkspaceFoldersChangeRequest';
import { DevToolsExtensionService } from './devToolsExtensionService';

export class WorkspaceChangeTrackingService extends DevToolsExtensionService {

    constructor(context: DevToolsExtensionContext) {
        super(context);
        this.registerEventHandlers();
        this.initializeWorkspace();
    }

    protected registerEventHandlers() {
        //workspace folders events
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidChangeWorkspaceFolders(e => {
                this._context.toolsLangServerClient.workspaceFolderChange(new ToolsWorkspaceFoldersChangeRequest(e.added, e.removed));
            }));

        //document events
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidOpenTextDocument(e => {
                this._context.toolsLangServerClient.documentOpen(new ToolsDocumentChangeRequest(e.uri.fsPath, undefined));
            }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidChangeTextDocument(async e => {                
                if ((e.document) && (e.document.uri)) {
                    let buildSymbols = (this._context.activeDocumentSymbols.isActiveDocument(e.document)) && (e.document.languageId == "al");
                    
                    let response = await this._context.toolsLangServerClient.documentChange(new ToolsDocumentContentChangeRequest(e.document.uri.fsPath, e.document.getText(), buildSymbols));                    
                    
                    if ((buildSymbols) && (response))
                        this._context.activeDocumentSymbols.loadFromAny(response.root);
                }
            }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidCloseTextDocument(e => {
                this._context.toolsLangServerClient.documentClose(new ToolsDocumentChangeRequest(e.uri.fsPath, undefined));
            }));
    
        let watcher =  vscode.workspace.createFileSystemWatcher("**/*"); //we are tracking all file changes to handle folders  .{al,app,json}");
        watcher.onDidChange(e => {
            this._context.toolsLangServerClient.fileSystemFileChange(new ToolsFileSystemFileChangeRequest(e.fsPath));
            //notify symbols service that the app file has changed and any files extracted from this file should be refreshed
            if (e.path.endsWith('.app'))
                this._context.symbolsService.appFileChanged(e);
        });
        watcher.onDidCreate(e => {
            this._context.toolsLangServerClient.fileSystemFileCreate(new ToolsFileSystemFileChangeRequest(e.fsPath));
        });
        watcher.onDidDelete(e => {
            this._context.toolsLangServerClient.fileSystemFileDelete(new ToolsFileSystemFileChangeRequest(e.fsPath));
        });
       
        this._context.vscodeExtensionContext.subscriptions.push(watcher);

        this._context.vscodeExtensionContext.subscriptions.push(vscode.workspace.onDidChangeConfiguration(configChange => {
            this.onConfigurationChange(configChange);
        }));
    }

    protected initializeWorkspace() {
        this._context.toolsLangServerClient.workspaceFolderChange(
            new ToolsWorkspaceFoldersChangeRequest(vscode.workspace.workspaceFolders, undefined));
    }

    protected onConfigurationChange(configChange: vscode.ConfigurationChangeEvent) {
        //collect configuration changes        
        let folders = vscode.workspace.workspaceFolders;
        if ((folders) && (folders.length > 0)) {
            let projectSources : ToolsALProjectSource[] = [];
            for (let i=0; i<folders.length; i++)
                if ((configChange.affectsConfiguration('al.packageCachePath', folders[i].uri)) || 
                    (configChange.affectsConfiguration('al.codeAnalyzers', folders[i].uri)) ||
                    (configChange.affectsConfiguration('alOutline.additionalMandatoryAffixesPatterns', folders[i].uri)))
                    projectSources.push(new ToolsALProjectSource(folders[i].uri));
            if (projectSources.length > 0)
                this._context.toolsLangServerClient.configurationChange(new ToolsConfigurationChangeRequest(projectSources));
        }
    }

}