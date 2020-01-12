import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";

export class ALSymbolsService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async loadDocumentSymbols(docUri: vscode.Uri) : Promise<AZDocumentSymbolsLibrary> {
        let symbolsLibrary: AZDocumentSymbolsLibrary = new AZDocumentSymbolsLibrary(this._context, docUri);
        await symbolsLibrary.loadAsync(false);
        return symbolsLibrary;
    }

}