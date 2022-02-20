import * as vscode from 'vscode';

export class ToolsALProjectSource {
    folderPath: string;
    packageCachePath: string | undefined;

    constructor(folderUri: vscode.Uri) {
        this.folderPath = folderUri.fsPath;
        let config = vscode.workspace.getConfiguration('al', folderUri);        
        this.packageCachePath = config.get<string>('packageCachePath');
    }
}