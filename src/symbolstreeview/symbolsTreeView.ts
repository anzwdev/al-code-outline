import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from "../webviews/baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';

export class SymbolsTreeView extends BaseWebViewEditor {
    protected _devToolsContext : DevToolsExtensionContext;
    private _symbolsChangedHandler : vscode.Disposable | undefined;
    protected _loaded: boolean;
    protected _rootSymbol: AZSymbolInformation;
    protected _documentName: string;
    protected _documentUri: vscode.Uri | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, symbol: AZSymbolInformation, documentName: string | undefined,
        rootSymbolName: string | undefined, documentUri: vscode.Uri | undefined) {
        if (!documentName) {
            if (documentUri)
                documentName = path.parse(documentUri.path).base;
        }
        if (!rootSymbolName)
            rootSymbolName = documentName;

        super(devToolsContext.vscodeExtensionContext, documentName);

        this._documentName = documentName;
        this._documentUri = documentUri;
        this._devToolsContext = devToolsContext;
        this._viewColumn = vscode.ViewColumn.Beside;
        this._loaded = false;
        this._rootSymbol = symbol.createCopy(true);
        if (rootSymbolName)
            this._rootSymbol.fullName = rootSymbolName;

        if (this._documentUri)
            this._symbolsChangedHandler = this._devToolsContext.activeDocumentSymbols.onSymbolsChanged(symbolsLib => this.onSymbolsChanged(symbolsLib));
        else
            this._symbolsChangedHandler = undefined;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'symbolstreeview', 'symbolstreeview.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.SymbolsTreeView';
    }

    protected async onDocumentLoaded() {
        this._loaded = true;
        await this.updateView();
    }

    onSymbolsChanged(lib: any) {
        if (this._devToolsContext.activeDocumentSymbols.getDocUri().path == this._documentUri.path) {
            if (this._devToolsContext.activeDocumentSymbols.rootSymbol)
                this._rootSymbol = this._devToolsContext.activeDocumentSymbols.rootSymbol.createCopy(true);
            else
                this._rootSymbol = new AZSymbolInformation();
            this._rootSymbol.fullName = this._documentName;
            this.updateView();        
        }
    }

    protected updateView() {
        if (!this._loaded)
            return;
        this.sendMessage({
            command: 'setData',
            data: this._rootSymbol
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;        

        return false;
    }

    protected onPanelClosed() {
        if (this._symbolsChangedHandler)
            this._symbolsChangedHandler.dispose();
    }

}