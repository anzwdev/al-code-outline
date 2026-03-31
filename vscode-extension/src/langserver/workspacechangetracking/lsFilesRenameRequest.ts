import * as vscode from 'vscode';
import { LSFileRenameDetails } from './lsFileRenameDetails';

export class LSFilesRenameRequest {
    files: LSFileRenameDetails[] | undefined;

    constructor(newFiles: readonly { oldUri: vscode.Uri, newUri: vscode.Uri }[]) {
        this.files = [];
        if (newFiles) {
            for (let i=0; i<newFiles.length; i++) {
                this.files.push(new LSFileRenameDetails(newFiles[i].oldUri, newFiles[i].newUri));
            }
        }
    }

}