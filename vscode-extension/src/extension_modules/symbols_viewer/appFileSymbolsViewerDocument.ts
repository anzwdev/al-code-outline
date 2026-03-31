import * as vscode from 'vscode';

export class AppFileSymbolsViewerDocument implements vscode.CustomDocument {
    uri: vscode.Uri;
    
    constructor(newUri: vscode.Uri) {
        this.uri = newUri;
    }
    
    dispose(): void {
    }

}