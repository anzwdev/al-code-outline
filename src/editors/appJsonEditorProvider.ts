import * as vscode from 'vscode';
import { AppJsonEditor } from './appJsonEditor';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';

export class AppJsonEditorProvider implements vscode.CustomTextEditorProvider {
    protected _devToolsContext: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        this._devToolsContext = devToolsContext;
    }

    public async resolveCustomTextEditor(document: vscode.TextDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): Promise<void> {
        let editor: AppJsonEditor = new AppJsonEditor(this._devToolsContext);
        editor.resolveCustomTextEditor(document, webviewPanel);
    }

}
