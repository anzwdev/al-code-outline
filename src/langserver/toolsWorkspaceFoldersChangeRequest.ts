import * as vscode from 'vscode';

export class ToolsWorkspaceFoldersChangeRequest {
    added: string[] | undefined;
    removed: string[] | undefined;

    constructor(addedFolders: readonly vscode.WorkspaceFolder[] | undefined, removedFolders: readonly vscode.WorkspaceFolder[] | undefined) {
        this.added = this.getFolders(addedFolders);
        this.removed = this.getFolders(removedFolders);
    }

    protected getFolders(wsFolders: readonly vscode.WorkspaceFolder[] | undefined): string[] | undefined {
        if (wsFolders) {
            let folders = [];
            for (let i=0; i<wsFolders.length; i++) {
                if ((wsFolders[i].uri) && (wsFolders[i].uri.fsPath))
                    folders.push(wsFolders[i].uri.fsPath);
            }
            return folders;
        }
        return undefined;
    }


}