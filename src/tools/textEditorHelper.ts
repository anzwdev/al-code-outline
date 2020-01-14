import * as vscode from 'vscode';

export class TextEditorHelper {

    static findDocumentEditor(docUri: vscode.Uri) : vscode.TextEditor | undefined {       
        if (docUri) {
            let docUriString : string = docUri.toString();

            if ((vscode.window.activeTextEditor) && 
                (vscode.window.activeTextEditor.document) && 
                (vscode.window.activeTextEditor.document.uri) &&
                (vscode.window.activeTextEditor.document.uri.toString() == docUriString))
                return vscode.window.activeTextEditor;

            let editors = vscode.window.visibleTextEditors;
            for (let i=0; i<editors.length; i++) {
                if ((editors[i].document) && (editors[i].document.uri)) {
                    let editorUri : string = editors[i].document.uri.toString();
                    if (editorUri == docUriString) {
                        return editors[i];
                    }
                }
            }
        }
        return undefined;
    }

    static async openEditor(docUri: vscode.Uri, reuseExisting: boolean, preview: boolean, position: vscode.Position | undefined) : Promise<vscode.TextEditor | undefined> {
        let editorViewColumn: vscode.ViewColumn | undefined = undefined;
        if (reuseExisting) {
            let editor = this.findDocumentEditor(docUri);
            if (editor)
                editorViewColumn = editor.viewColumn;
        }

        try {
            let targetDoc = await vscode.workspace.openTextDocument(docUri);
            let targetEditor = await vscode.window.showTextDocument(targetDoc, {
                preview : preview,
                viewColumn : editorViewColumn  
            });

            if (position) {
                targetEditor.selection = new vscode.Selection(position, position);
                targetEditor.revealRange(targetEditor.selection);
            }

            return targetEditor;
        }
        catch (e) {
            vscode.window.showErrorMessage(e.message);
        }
        return undefined;
    }

    static async showNewDocument(content: string, language: string) {
        try {
            let document = await vscode.workspace.openTextDocument({
                content : content,
                language : language
            });
            vscode.window.showTextDocument(document, {
                    preview : false
                });
        }
        catch (e) {
            vscode.window.showErrorMessage(e.message);
        }
    }

    static getActiveWorkspaceFolderUri(): vscode.Uri | undefined {
        let folder: vscode.WorkspaceFolder | undefined = undefined;
        
        if ((vscode.window.activeTextEditor) && (vscode.window.activeTextEditor.document) && (vscode.window.activeTextEditor.document.uri)) {
            folder = vscode.workspace.getWorkspaceFolder(vscode.window.activeTextEditor.document.uri);
            if ((folder) && (folder.uri))
                return folder.uri;
        }

        let editors = vscode.window.visibleTextEditors;
        for (let i=0; i<editors.length; i++) {
            if ((editors[i].document) && (editors[i].document.uri)) {
                folder = vscode.workspace.getWorkspaceFolder(editors[i].document.uri);
                if ((folder) && (folder.uri))
                    return folder.uri;
            }
        }

        return vscode.workspace.workspaceFolders[0].uri;
    }

}