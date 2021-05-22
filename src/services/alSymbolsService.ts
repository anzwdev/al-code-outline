import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AppFileTextContentProvider } from '../editorextensions/appFileTextContentProvider';
import { AZDocumentSymbolsLibrary } from "../symbollibraries/azDocumentSymbolsLibrary";

export class ALSymbolsService {
    protected _context: DevToolsExtensionContext;
    protected _appFilteTextContentProvider: AppFileTextContentProvider; 

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
        this._appFilteTextContentProvider = new AppFileTextContentProvider(this._context);

        this.registerServices();
    }

    protected registerServices() {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.workspace.registerTextDocumentContentProvider(AppFileTextContentProvider.scheme, this._appFilteTextContentProvider));
    }

    async loadDocumentSymbols(docUri: vscode.Uri) : Promise<AZDocumentSymbolsLibrary> {
        let symbolsLibrary: AZDocumentSymbolsLibrary = new AZDocumentSymbolsLibrary(this._context, docUri);
        await symbolsLibrary.loadAsync(false);
        return symbolsLibrary;
    }

    appFileChanged(uri: vscode.Uri) {
        this._appFilteTextContentProvider.appFileChanged(uri);
    }

}