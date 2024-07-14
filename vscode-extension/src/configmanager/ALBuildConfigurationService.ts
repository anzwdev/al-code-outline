import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DevToolsExtensionService } from '../services/devToolsExtensionService';
import { ALWorkspaceFolderBuildConfigurations } from './ALWorkspaceFolderBuildConfigurations';

export class ALBuildConfigurationService extends DevToolsExtensionService {
    activeFolderConfigurations: ALWorkspaceFolderBuildConfigurations | undefined = undefined;
    workspaceFoldersConfigurations: ALWorkspaceFolderBuildConfigurations[] = [];
    statusBarItem: vscode.StatusBarItem;
    statusBarItemVisible: boolean = false;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        //subscribe to events
        this._context.vscodeExtensionContext.subscriptions.push(vscode.workspace.onDidSaveTextDocument(this.onDidSaveTextDocument, this));
        this._context.vscodeExtensionContext.subscriptions.push(vscode.workspace.onDidCreateFiles(this.onDidCreateFiles, this));
        this._context.vscodeExtensionContext.subscriptions.push(vscode.workspace.onDidDeleteFiles(this.onDidDeleteFiles, this));
        this._context.vscodeExtensionContext.subscriptions.push(vscode.workspace.onDidChangeWorkspaceFolders(this.onDidChangeWorkspaceFoldersAsync, this));
        this._context.vscodeExtensionContext.subscriptions.push(vscode.window.onDidChangeActiveTextEditor(this.onDidChangeActiveTextEditor, this));
        this._context.vscodeExtensionContext.subscriptions.push(vscode.workspace.onDidChangeConfiguration(this.onDidChangeConfigurationAsync, this));

        //define new toolbar dropdown command
        this._context.vscodeExtensionContext.subscriptions.push(vscode.commands.registerCommand('azALDevTools.selectBuildConfiguration', async () => {
            await this.selectBuildConfigurationAsync();
        }));

        //create statusbar item
        this.statusBarItem = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Left, 1);
        this._context.vscodeExtensionContext.subscriptions.push(this.statusBarItem);
        this.statusBarItem.command = 'azALDevTools.selectBuildConfiguration';
        this.statusBarItem.tooltip = 'Select active app.json file';

        this.loadConfigurationsAsync();
    }
    
    private async selectBuildConfigurationAsync() {
        if (this.activeFolderConfigurations !== undefined) {
            let buildConfigurations = [];
            for (let i=0; i<this.activeFolderConfigurations.configurations.length; i++) {
                buildConfigurations.push({
                    label: this.activeFolderConfigurations.configurations[i].name,
                    description: this.activeFolderConfigurations.configurations[i].fileName,
                    value: this.activeFolderConfigurations.configurations[i]
                });
            }
            let selectedBuildConfiguration = await vscode.window.showQuickPick(buildConfigurations, {
                title: 'Select active build configuration (app.json)',
                placeHolder: 'Select active app.json file'
            });

            if (selectedBuildConfiguration) {
                await this.activeFolderConfigurations.setActiveConfigurationAsync(selectedBuildConfiguration.value);
                this.updateStatusBar();
            }
        }
    }

    private async loadConfigurationsAsync() {
        this.workspaceFoldersConfigurations = [];
        let workspaceFolders = vscode.workspace.workspaceFolders;
        if (workspaceFolders) {
            for (let i = 0; i < workspaceFolders.length; i++) {
                let folderConfig = new ALWorkspaceFolderBuildConfigurations(workspaceFolders[i].uri);
                this.workspaceFoldersConfigurations.push(folderConfig);
                await folderConfig.initAsync();
            }
        }
        this.updateActiveFolderConfiguration();
    }

    private updateActiveFolderConfiguration() {
        this.activeFolderConfigurations = undefined;
        if ((this.workspaceFoldersConfigurations.length > 1) && (vscode.window.activeTextEditor) && (vscode.window.activeTextEditor.document) && (vscode.window.activeTextEditor.document.uri)) {
            let workspaceFolder = vscode.workspace.getWorkspaceFolder(vscode.window.activeTextEditor.document.uri);
            if (workspaceFolder) {
                let idx = this.findWorkspaceFolderIndex(workspaceFolder.uri);
                if (idx >= 0) {
                    this.activeFolderConfigurations = this.workspaceFoldersConfigurations[idx];
                }
            }
        }

        if ((this.activeFolderConfigurations === undefined) && (this.workspaceFoldersConfigurations.length > 0)) {
            this.activeFolderConfigurations = this.workspaceFoldersConfigurations[0];
        }

        this.updateStatusBar();
    }

    onDidSaveTextDocument(e: vscode.TextDocument) {
        let workspaceFolder = vscode.workspace.getWorkspaceFolder(e.uri);
        if (workspaceFolder) {
            let idx = this.findWorkspaceFolderIndex(workspaceFolder.uri);
            if (idx >= 0) {
                this.workspaceFoldersConfigurations[idx].onFileSavedAsync(e.uri);
            }
        }
    }

    onDidCreateFiles(e: vscode.FileCreateEvent) {
        e.files.forEach((fileUri) => {
            let workspaceFolder = vscode.workspace.getWorkspaceFolder(fileUri);
            if (workspaceFolder) {
                let idx = this.findWorkspaceFolderIndex(workspaceFolder.uri);
                if (idx >= 0) {
                    this.workspaceFoldersConfigurations[idx].onFileCreated(fileUri);
                }
            }
        });
        this.updateStatusBar();
    }

    onDidDeleteFiles(e: vscode.FileDeleteEvent) {
        e.files.forEach((fileUri) => {
            let workspaceFolder = vscode.workspace.getWorkspaceFolder(fileUri);
            if (workspaceFolder) {
                let idx = this.findWorkspaceFolderIndex(workspaceFolder.uri);
                if (idx >= 0) {
                    this.workspaceFoldersConfigurations[idx].onFileDeletedAsync(fileUri);
                }
            }
        });
        this.updateStatusBar();
    }

    onDidChangeActiveTextEditor(e: vscode.TextEditor | undefined) {
        this.updateActiveFolderConfiguration();
    }

    async onDidChangeConfigurationAsync(e: vscode.ConfigurationChangeEvent) {
        for (let i=0; i<this.workspaceFoldersConfigurations.length; i++) {
            await this.workspaceFoldersConfigurations[i].onDidChangeConfigurationAsync(e);
        }
        this.updateStatusBar();
    }

    async onDidChangeWorkspaceFoldersAsync(e: vscode.WorkspaceFoldersChangeEvent) {
        e.added.forEach(async (addedFolder) => {
            let folderConfig = new ALWorkspaceFolderBuildConfigurations(addedFolder.uri);
            this.workspaceFoldersConfigurations.push(folderConfig);
            await folderConfig.initAsync();
        });

        e.removed.forEach((removedFolder) => {
            let folderIndex = this.findWorkspaceFolderIndex(removedFolder.uri);
            if (folderIndex >= 0) {
                this.workspaceFoldersConfigurations.splice(folderIndex, 1);
            }
        });

        this.updateActiveFolderConfiguration();
    }

    updateStatusBar() {
        if ((this.activeFolderConfigurations !== undefined) && (this.activeFolderConfigurations.configurations.length > 0)) {
            let newText = this.statusBarItem.text;
            if (this.activeFolderConfigurations.active !== undefined) {
                newText = '$(gear) app.json: ' + this.activeFolderConfigurations.active.name;
            } else {
                newText = '$(gear) app.json: No configuration selected';
            }
            if (newText !== this.statusBarItem.text) {
                this.statusBarItem.text = newText;
            }
            if (!this.statusBarItemVisible) {
                this.statusBarItem.show();
                this.statusBarItemVisible = true;
            }
        } else if (this.statusBarItemVisible) {
            this.statusBarItem.hide();
            this.statusBarItemVisible = false;
        }
    }

    findWorkspaceFolderIndex(uri: vscode.Uri) {
        for (let i = 0; i < this.workspaceFoldersConfigurations.length; i++) {
            if (this.workspaceFoldersConfigurations[i].uri === uri) {
                return i;
            }
        }
        return -1;
    }

}