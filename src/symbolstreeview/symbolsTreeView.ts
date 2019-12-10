import * as vscode from 'vscode';
import * as path from 'path';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { BaseSymbolsWebView } from '../webviews/baseSymbolsWebView';

export class SymbolsTreeView extends BaseSymbolsWebView {
    selectedSymbolRange: vscode.Range | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, documentName: string | undefined, documentUri: vscode.Uri | undefined) {        
        super(devToolsContext, documentName, documentUri);
        this._copySymbols = true;

        if (this._documentUri)
            this._disposables.push(this._devToolsContext.activeDocumentSymbols.onSymbolsChanged(symbolsLib => this.onSymbolsChanged(symbolsLib)));
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'symbolstreeview', 'symbolstreeview.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.SymbolsTreeView';
    }

    protected async loadSymbols() {
        let currDocUri = this._devToolsContext.activeDocumentSymbols.getDocUri();
        if ((currDocUri) && (currDocUri.toString() == this._documentUri.toString()) && (this._devToolsContext.activeDocumentSymbols.rootSymbol)) {
            this._selectedSymbolPath = this._devToolsContext.activeDocumentSymbols.findSymbolPathInSelectionRange(this.selectedSymbolRange);            
            this.setSymbols(this._devToolsContext.activeDocumentSymbols.rootSymbol, this._title);
        } else {
            let library = new AZDocumentSymbolsLibrary(this._devToolsContext, this._documentUri);
            await library.loadAsync(false);            
            this._selectedSymbolPath = library.findSymbolPathInSelectionRange(this.selectedSymbolRange);
            this.setSymbols(library.rootSymbol, this._title);
        }
    }

    onSymbolsChanged(lib: any) {
        if (this._devToolsContext.activeDocumentSymbols.getDocUri().path == this._documentUri.path) {
            this._selectedSymbolPath = undefined;
            this.setSymbols(this._devToolsContext.activeDocumentSymbols.rootSymbol, this._title);
        }
    }

    protected onPanelClosed() {
        if (this._documentUri)
            this._devToolsContext.alSymbolsTreeService.removeUriSymbolsTreeView(this._documentUri);
    }
    

}