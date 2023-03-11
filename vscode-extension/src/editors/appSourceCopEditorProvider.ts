import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AppSourceCopEditor } from './appSourceCopEditor';

export class AppSourceCopEditorProvider implements vscode.CustomTextEditorProvider {
    protected _devToolsContext: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        this._devToolsContext = devToolsContext;
    }

    public async resolveCustomTextEditor(document: vscode.TextDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): Promise<void> {
        let editor: AppSourceCopEditor = new AppSourceCopEditor(this._devToolsContext);
        editor.resolveCustomTextEditor(document, webviewPanel);
    }

}