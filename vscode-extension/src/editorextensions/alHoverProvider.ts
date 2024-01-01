import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ToolsDocumentPositionRequest } from '../langserver/toolsDocumentPositionRequest';

export class ALHoverProvider implements vscode.HoverProvider {

    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async provideHover(document: vscode.TextDocument, position: vscode.Position, token: vscode.CancellationToken): Promise<vscode.Hover | undefined> {
        let isActive = (vscode.window.activeTextEditor?.document?.uri.fsPath === document.uri.fsPath);
        let source: string | undefined = undefined;
        if (!isActive)
            source = document.getText();
        else
            await this._context.activeDocumentSymbols.loadAsync(false);        

        let request = new ToolsDocumentPositionRequest(isActive, source, position);
        let response = await this._context.toolsLangServerClient.provideHover(request);

        if ((response) && (response.hover) && (response.hover.length > 0))
            return new vscode.Hover(response.hover);

        return undefined;
    }

}