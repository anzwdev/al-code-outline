import * as vscode from 'vscode';

export class ALObjectWizardSettings {
    destDirectoryPath: string | undefined;
    destFileName: string | undefined;    
    private _destDirectoryUri: vscode.Uri | undefined;

    constructor() {
    }

    getDestDirectoryUri() : vscode.Uri | undefined {
        if (!this.destDirectoryPath)
            return undefined;
        if (!this._destDirectoryUri)
            this._destDirectoryUri = vscode.Uri.file(this.destDirectoryPath);
        return this._destDirectoryUri;
    }  


}