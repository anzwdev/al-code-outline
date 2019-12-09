import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { ToolsGetSyntaxTreeRequest } from '../langserver/toolsGetSyntaxTreeRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { ToolsGetSyntaxTreeSymbolsRequest } from '../langserver/toolsGetSyntaxTreeSymbolRequest';
import { ToolsCloseSyntaxTreeRequest } from '../langserver/toolsCloseSyntaxTreeRequest';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';

export class SyntaxTreeView extends BaseWebViewEditor {
    protected _devToolsContext : DevToolsExtensionContext;
    protected _loaded: boolean;
    protected _rootSymbol: AZSymbolInformation | undefined;
    protected _documentUri: vscode.Uri | undefined;
    protected _firstLoad: boolean;

    constructor(devToolsContext : DevToolsExtensionContext, documentUri: vscode.Uri | undefined) {        
        let documentName = path.parse(documentUri.path).base;
        super(devToolsContext.vscodeExtensionContext, documentName);
        
        this._documentUri = documentUri;
        this._devToolsContext = devToolsContext;
        this._loaded = false;
        this._rootSymbol = undefined;
        this._viewColumn = vscode.ViewColumn.Beside;
        this._firstLoad = true;
        
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'syntaxtreeview', 'syntaxtreeview.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.SyntaxTreeView';
    }

    protected async onDocumentLoaded() {
        this._loaded = true;
        if (this._documentUri)
            await this.loadSymbols();
        else
            await this.updateView();
    }

    protected async loadSymbols() {
        let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
        let source: string = '';
        if (editor)
            source = editor.document.getText();
        
        //let fsPath = this._documentUri.fsPath;
        let request: ToolsGetSyntaxTreeRequest = new ToolsGetSyntaxTreeRequest(source, this._documentUri.fsPath, this._firstLoad);
        this._firstLoad = false;
        let response = await this._devToolsContext.toolsLangServerClient.getSyntaxTree(request);
        if (response)
            this.setSymbols(response.root);
        else
            this.setSymbols(undefined);
    }

    protected async onSymbolSelected(symbolPath: number[]) {
        let request: ToolsGetSyntaxTreeSymbolsRequest = new ToolsGetSyntaxTreeSymbolsRequest(this._documentUri.fsPath, symbolPath);
        let response = await this._devToolsContext.toolsLangServerClient.getSyntaxTreeSymbol(request);
        if (response) {
            this.setSelectedSymbol(response.symbol);
            if (response.symbol) {
                let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
                if (editor) {
                    editor.selection = new vscode.Selection(response.symbol.range.start.line,
                        response.symbol.range.start.character,
                        response.symbol.range.end.line,
                        response.symbol.range.end.character);
                    editor.revealRange(editor.selection);
                }
            }

        } else
            this.setSelectedSymbol(undefined);
    }

    setSymbols(rootSymbol: any) {
        if (rootSymbol) {
            this._rootSymbol = AZSymbolInformation.fromAny(rootSymbol);
            this._rootSymbol.updateTree(true, false);
        } else
            this._rootSymbol = new AZSymbolInformation();
        this.updateView();
    }

    setSelectedSymbol(symbol: any) {
        this.sendMessage({
            command: 'setSymbolInfo',
            data: symbol
        });
    }

    protected updateView() {
        if (!this._loaded)
            return;
        this.sendMessage({
            command: 'setData',
            data: this._rootSymbol,
            selected: this.getPathAtCursor()
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;        

        if (message) {
            switch (message.command) {
                case 'symbolselected':
                    this.onSymbolSelected(message.path);
                    return true;
                case 'refresh':
                    this.loadSymbols();
                    return true;
                case 'sync':
                    this.syncPos();
                    return true;
            }
        }

        return false;
    }

    protected getPathAtCursor() : number[] | undefined {
        let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
        if ((editor) && (this._rootSymbol)) {
            let library = new AZDocumentSymbolsLibrary(this._devToolsContext, this._documentUri);
            library.setRootSymbol(this._rootSymbol);
            return library.findSymbolPathInRange(editor.selection);
        }
        return undefined;
    }

    protected syncPos() {
        let selectedPath : number[] | undefined = this.getPathAtCursor();
        if (selectedPath) {
            this.sendMessage({
                command: 'selectSymbol',
                selected: selectedPath
            });
        }
    }

    protected onPanelClosed() {
        super.onPanelClosed();
        let request: ToolsCloseSyntaxTreeRequest = new ToolsCloseSyntaxTreeRequest(this._documentUri.fsPath);
        this._devToolsContext.toolsLangServerClient.closeSyntaxTree(request);   
    }
}