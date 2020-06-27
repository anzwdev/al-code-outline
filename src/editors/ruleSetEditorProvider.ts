import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { RuleSetEditor } from './ruleSetEditor';

export class RuleSetEditorProvider implements vscode.CustomTextEditorProvider {
    protected _devToolsContext: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        this._devToolsContext = devToolsContext;
    }

    public async resolveCustomTextEditor(document: vscode.TextDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): Promise<void> {
        let editor: RuleSetEditor = new RuleSetEditor(this._devToolsContext);
        editor.resolveCustomTextEditor(document, webviewPanel);
    }

}