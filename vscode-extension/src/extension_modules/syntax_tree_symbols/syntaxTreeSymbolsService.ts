import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DocumentInformationService } from '../documentInformationService';
import { LSSyntaxTreeSymbolsClient } from '../../langserver/syntaxtreesymbols/lsSyntaxTreeSymbolsClient';

// Service to retrieve symbols built from syntax trees processing
// It is not a raw, syntax tree structure, but a simplified one, with only the relevant information for the symbols tree view (like kind of node, name, position, etc).

export class SyntaxTreeSymbolsService extends DocumentInformationService<any> {
      
    private _syntaxTreeSymbolsClient: LSSyntaxTreeSymbolsClient;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._syntaxTreeSymbolsClient = new LSSyntaxTreeSymbolsClient(context.lsConnector);
    }

    protected override async loadContent(documentUri: vscode.Uri | undefined): Promise<any | undefined> {
        if ((documentUri) && (documentUri.fsPath)) {
            let documentContent: string | undefined = undefined;
            let documentPath: string | undefined = documentUri.fsPath;

            if (documentUri.scheme === "al-preview") {
                let document = await vscode.workspace.openTextDocument(documentUri);
                documentContent = document.getText();
                documentPath = undefined;
            } 

            let content = await this._syntaxTreeSymbolsClient.getSymbols({ path: documentPath, content: documentContent });
            return content?.rootNode;            
            
        }
        return undefined;
    }

}
