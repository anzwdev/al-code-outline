import * as vscode from 'vscode';

export class WorkspaceFolderQuickPickItem implements vscode.QuickPickItem {
    label: string;
    description?: string | undefined;
    detail?: string | undefined;
    picked?: boolean | undefined;
    alwaysShow?: boolean | undefined;
    folder: vscode.WorkspaceFolder | undefined;

    constructor(newFolder: vscode.WorkspaceFolder | undefined) {
        this.folder = newFolder;
        if (this.folder) {
            this.label = this.folder.name;
            this.description = this.folder.uri.fsPath;          
        } else {
            this.label = "All workspace folders";
            this.description = undefined;
        }
    }

}