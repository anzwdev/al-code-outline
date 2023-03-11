import * as vscode from 'vscode';
import { ToolsFileRenameDetailsRequest } from "./toolsFilesRenameDetailsRequest";

export class ToolsFilesRenameRequest {
    files: ToolsFileRenameDetailsRequest[] | undefined;

    constructor(newFiles: readonly { oldUri: vscode.Uri, newUri: vscode.Uri }[]) {
        this.files = [];
        if (newFiles) 
            for (let i=0; i<newFiles.length; i++) {
                this.files.push(new ToolsFileRenameDetailsRequest(newFiles[i].oldUri, newFiles[i].newUri));
            }

    }

}