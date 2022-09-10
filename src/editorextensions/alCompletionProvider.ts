import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { CodeCompletionItem } from '../langserver/codeCompletion/codeCompletionItem';
import { ToolsCodeCompletionRequest } from '../langserver/codeCompletion/toolsCodeCompletionRequest';
import { TextPosition } from '../symbollibraries/textPosition';

export class ALCompletionProvider implements vscode.CompletionItemProvider {

    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async provideCompletionItems(document: vscode.TextDocument, position: vscode.Position, token: vscode.CancellationToken, context: vscode.CompletionContext): Promise<vscode.CompletionItem[]> {
        let completionItems: vscode.CompletionItem[] = [];

        if (document.uri?.fsPath) {
            let configuration = vscode.workspace.getConfiguration('alOutline', document.uri);
            let completionProviders = configuration.get<string[]>('completionProviders');

            if ((completionProviders) && (completionProviders.length > 0)) {
                await this._context.activeDocumentSymbols.loadAsync(false);

                let textPosition = new TextPosition();
                textPosition.set(position.line, position.character);
                let request = new ToolsCodeCompletionRequest(textPosition, document.uri.fsPath, completionProviders);
                let response = await this._context.toolsLangServerClient.codeCompletion(request);
                
                if ((response) && (response.completionItems)) {
                    for (let i=0; i<response.completionItems.length; i++)
                        if (response.completionItems[i].label)
                            completionItems.push(this.createCompletionItem(response.completionItems[i]));
                }
            }
        }

        return completionItems;
    }

    private createCompletionItem(source: CodeCompletionItem) {
        let item = new vscode.CompletionItem(source.label!, source.kind);
        item.filterText = source.filterText;
        if (source.tags)
            item.tags = source.tags;
        return item;
    }
    
}