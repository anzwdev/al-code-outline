import * as vscode from 'vscode';

export class ObjectBuilder {
    
    constructor() {
    }

    protected showNewDocument(content : string) {
        vscode.workspace.openTextDocument({
            content : content,
            language : "al"
        }).then(
            document => { 
                vscode.window.showTextDocument(document, {
                    preview : false
                });
            },
            err => {
                vscode.window.showErrorMessage(err);
            });
    }

}