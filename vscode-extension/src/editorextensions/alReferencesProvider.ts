import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ToolsDocumentPositionRequest } from '../langserver/toolsDocumentPositionRequest';

export class ALReferencesProvider implements vscode.ReferenceProvider {

    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async provideReferences(document: vscode.TextDocument, position: vscode.Position, context: vscode.ReferenceContext, token: vscode.CancellationToken): Promise<vscode.Location[] | undefined> {
        let isActive = (vscode.window.activeTextEditor?.document?.uri.fsPath === document.uri.fsPath);
        let source: string | undefined = undefined;
        if (!isActive)
            source = document.getText();
        else
            await this._context.activeDocumentSymbols.loadAsync(false);        

        let request = new ToolsDocumentPositionRequest(isActive, source, position);
        let response = await this._context.toolsLangServerClient.provideReferences(request);

        if ((response) && (response.references) && (response.references.length > 0)) {
            let locations: vscode.Location[] = [];
            for (let i=0; i<response.references.length; i++) {
                let ref = response.references[i];
                if ((ref.filePath) && (ref.start) && (ref.end))
                    locations.push(new vscode.Location(
                        vscode.Uri.file(ref.filePath),
                        new vscode.Range(ref.start.line, ref.start.character, ref.end.line, ref.end.character)));
            }
            return locations;
        }
          
        return undefined;
    }

}