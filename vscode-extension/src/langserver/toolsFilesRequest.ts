import * as vscode from 'vscode';

export class ToolsFilesRequest {
    files: string[] | undefined;

    constructor(newFiles: readonly vscode.Uri[]) {
        this.files = [];
        if (newFiles)
            for (let i=0; i<newFiles.length; i++) {
                if (newFiles[i].fsPath)
                    this.files.push(newFiles[i].fsPath);
            }
    }

}