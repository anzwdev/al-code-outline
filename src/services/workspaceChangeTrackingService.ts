import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsDocumentChangeRequest } from '../langserver/toolsDocumentChangeRequest';
import { ToolsDocumentContentChangeRequest } from '../langserver/toolsDocumentContentChangeRequest';
import { ToolsFileSystemFileChangeRequest } from '../langserver/toolsFileSystemFileChangeRequest';
import { ToolsWorkspaceFoldersChangeRequest } from '../langserver/toolsWorkspaceFoldersChangeRequest';

export class WorkspaceChangeTrackingService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
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
    
        /*
        //These events are handled by file system watcher
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidSaveTextDocument(e => {
                this._context.toolsLangServerClient.documentSave(new ToolsDocumentChangeRequest(e.uri.fsPath, undefined));
            }));

        //file events started from vs code ui
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidCreateFiles(e => {             
                this._context.toolsLangServerClient.fileCreate(new ToolsFilesRequest(e.files));
            }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidDeleteFiles(e => {
                this._context.toolsLangServerClient.fileDelete(new ToolsFilesRequest(e.files));
            }));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.onDidRenameFiles(e => {
                this._context.toolsLangServerClient.fileRename(new ToolsFilesRenameRequest(e.files));
            }));   
        */

        let watcher =  vscode.workspace.createFileSystemWatcher("**/*"); //we are tracking all file changes to handle folders  .{al,app,json}");
        watcher.onDidChange(e => {
            this._context.toolsLangServerClient.fileSystemFileChange(new ToolsFileSystemFileChangeRequest(e.fsPath));
        });
        watcher.onDidCreate(e => {
            this._context.toolsLangServerClient.fileSystemFileCreate(new ToolsFileSystemFileChangeRequest(e.fsPath));
        });
        watcher.onDidDelete(e => {
            this._context.toolsLangServerClient.fileSystemFileDelete(new ToolsFileSystemFileChangeRequest(e.fsPath));
        });

        this._context.vscodeExtensionContext.subscriptions.push(watcher);
    }

    protected initializeWorkspace() {
        this._context.toolsLangServerClient.workspaceFolderChange(
            new ToolsWorkspaceFoldersChangeRequest(vscode.workspace.workspaceFolders, undefined));
    }

}