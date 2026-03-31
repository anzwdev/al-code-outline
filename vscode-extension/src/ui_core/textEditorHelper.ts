import * as vscode from 'vscode';

export class TextEditorHelper {

    static findDocumentEditor(docUri: vscode.Uri | undefined) : vscode.TextEditor | undefined {       
        if (docUri) {
            let docUriString : string = docUri.toString();

            if ((vscode.window.activeTextEditor) && 
                (vscode.window.activeTextEditor.document) && 
                (vscode.window.activeTextEditor.document.uri) &&
                (vscode.window.activeTextEditor.document.uri.toString() === docUriString)) {
                return vscode.window.activeTextEditor;
            }

            let editors = vscode.window.visibleTextEditors;
            for (let i=0; i<editors.length; i++) {
                if ((editors[i].document) && (editors[i].document.uri)) {
                    let editorUri : string = editors[i].document.uri.toString();
                    if (editorUri === docUriString) {
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
            if (editor) {
                editorViewColumn = editor.viewColumn;
            }
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
        catch (e: any) {
            vscode.window.showErrorMessage(e.message);
        }
        return undefined;
    }



}