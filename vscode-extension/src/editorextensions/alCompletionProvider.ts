import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { CodeCompletionItem } from '../langserver/codeCompletion/codeCompletionItem';
import { ToolsCodeCompletionRequest } from '../langserver/codeCompletion/toolsCodeCompletionRequest';
import { TextPosition } from '../symbollibraries/textPosition';
import { CodeCompletionTextEdit } from '../langserver/codeCompletion/codeCompletionTextEdit';

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
            let keepVariableNamesAffixes = !!configuration.get<boolean>('keepVariableNamesCompletionAffixes');

            if ((completionProviders) && (completionProviders.length > 0)) {
                await this._context.activeDocumentSymbols.loadAsync(false);

                let textPosition = new TextPosition();
                textPosition.set(position.line, position.character);
                let request = new ToolsCodeCompletionRequest(textPosition, document.uri.fsPath, completionProviders, { 
                    keepVariableNamesAffixes: keepVariableNamesAffixes });
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
        let item = new vscode.CompletionItem({
            label: source.label!,
            detail: source.detail,
            description: source.description
        }, source.kind);
        item.filterText = source.filterText;

        if (source.tags) {
            item.tags = source.tags;
        }

        if (source.insertText) {
            item.insertText = source.insertText;
        }

        if (source.detail) {
            item.detail = source.detail;
        }

        if (source.commitCharacters) {
            item.commitCharacters = source.commitCharacters;
        }

        if (source.additionalTextEdits) {
            item.additionalTextEdits = this.createTextEdits(source.additionalTextEdits);
        }

        return item;
    }

    private createTextEdits(source: CodeCompletionTextEdit[]): vscode.TextEdit[] {
        let textEdits: vscode.TextEdit[] = [];
        if (source) {
            for (let i=0; i<source.length; i++) {
                let textEdit = this.createTextEdit(source[i]);
                if (textEdit) {
                    textEdits.push(textEdit);
                }
            }
        }
        return textEdits;
    }

    private createTextEdit(source: CodeCompletionTextEdit): vscode.TextEdit | undefined {
        if ((source.range) && (source.newText)) {
            let range = new vscode.Range(source.range.start.line, source.range.start.character, source.range.end.line, source.range.end.character);
            let textEdit = new vscode.TextEdit(range, source.newText);
            return textEdit;
        }
        return undefined;
    }

}