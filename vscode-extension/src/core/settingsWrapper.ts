import * as vscode from 'vscode';

export class SettingsWrapper {

    protected configuration: vscode.WorkspaceConfiguration;

    constructor(configurationSection: string, uri: vscode.Uri | undefined) {
        this.configuration = vscode.workspace.getConfiguration(configurationSection, uri);
    }

}