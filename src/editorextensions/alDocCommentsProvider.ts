import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { XmlHelper } from '../tools/xmlHelper';

export class ALDocCommentsProvider implements vscode.CompletionItemProvider {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    provideCompletionItems(document: vscode.TextDocument, position: vscode.Position, token: vscode.CancellationToken, context: vscode.CompletionContext): vscode.ProviderResult<vscode.CompletionItem[] | vscode.CompletionList> {
        let completionItems: vscode.CompletionItem[] = [];

        let docCommentsType = vscode.workspace.getConfiguration('alOutline', document.uri).get<string>('docCommentsType');
        if (docCommentsType == 'xml') {
            //documentation completion
            let lineStart = document.getText(new vscode.Range(position.line, 0, position.line, position.character));
            if ((lineStart.trim() == '///') && (!this.hasXmlComments(document, position.line - 1)) && (!this.hasXmlComments(document, position.line + 1))) {
                let symbol : AZSymbolInformation | undefined = this._context.activeDocumentSymbols.findNextSymbol(position.line + 1);
                if (symbol) {
                    let documentationText = ' <summary>\n/// $1\n/// </summary>';
                    let snippetParamIdx = 1;

                    if ((symbol.kind == AZSymbolKind.MethodDeclaration) ||
                        (symbol.kind == AZSymbolKind.LocalMethodDeclaration)) {
                        //function parameters
                        let parameters: AZSymbolInformation[] = [];
                        symbol.collectChildSymbols(AZSymbolKind.Parameter, parameters);
                        for (let idx = 0; idx < parameters.length; idx++) {
                            snippetParamIdx++;
                            documentationText = documentationText + '\n/// <param name="' +
                                XmlHelper.EncodeXmlAttributeValue(parameters[idx].name) + 
                                '">$' + snippetParamIdx.toString() +
                                '</param>';
                        }

                        //function return value
                        let valPos = symbol.fullName.lastIndexOf(")");
                        if (valPos >= 0) {
                            let retTypeText = symbol.fullName.substr(valPos);
                            if (retTypeText.startsWith(') :')) {
                                snippetParamIdx++;
                                documentationText = documentationText + '\n/// <returns>$' + snippetParamIdx.toString() + '</returns>';
                            }
                        }
                    }

                    let item: vscode.CompletionItem = new vscode.CompletionItem('Xml Documentation Comments', vscode.CompletionItemKind.Text);
                    item.insertText = new vscode.SnippetString(documentationText);
                    completionItems.push(item);    
                }

            }
        }

        return completionItems;
    }

    protected hasXmlComments(document: vscode.TextDocument, line: number) : boolean {
        if (line < 0)
            return false;
        let text = document.getText(new vscode.Range(line, 0, line + 1, 0)).trim();
        return text.startsWith('///');
    }
    


}