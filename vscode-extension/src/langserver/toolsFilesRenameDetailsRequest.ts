import * as vscode from 'vscode';

export class ToolsFileRenameDetailsRequest {
    oldPath: string | undefined;
    newPath: string | undefined;
    
    constructor(oldFile: vscode.Uri, newFile: vscode.Uri) {
        if (oldFile)
            this.oldPath = oldFile.fsPath;
        if (newFile)
            this.newPath = newFile.fsPath;
    }

}