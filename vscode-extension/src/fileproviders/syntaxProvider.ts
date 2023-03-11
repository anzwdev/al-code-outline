import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';

export class SyntaxProvider {
    name: string;
    protected _context: DevToolsExtensionContext;

    constructor(newContext: DevToolsExtensionContext, newName: string) {
        this._context = newContext;
        this.name = newName;
    }

    async provideTextDocumentContent(uri: vscode.Uri, token: vscode.CancellationToken): Promise<string> {
        return "";
    }

}