import * as vscode from 'vscode';
import { ToolsALProjectSource } from './toolsALProjectSource';

export class ToolsWorkspaceFoldersChangeRequest {
    added: ToolsALProjectSource[] | undefined;
    removed: string[] | undefined;

    constructor(addedFolders: readonly vscode.WorkspaceFolder[] | undefined, removedFolders: readonly vscode.WorkspaceFolder[] | undefined) {
        this.added = this.getProjectSources(addedFolders);
        this.removed = this.getFolders(removedFolders);
    }

    protected getProjectSources(wsFolders: readonly vscode.WorkspaceFolder[] | undefined): ToolsALProjectSource[] | undefined {
        if (wsFolders) {
            let folders : ToolsALProjectSource[] = [];
            for (let i=0; i<wsFolders.length; i++) {
                if ((wsFolders[i].uri) && (wsFolders[i].uri.fsPath))
                    folders.push(new ToolsALProjectSource(wsFolders[i].uri));
            }
            return folders;
        }
        return undefined;
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