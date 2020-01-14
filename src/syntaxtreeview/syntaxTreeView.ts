import * as vscode from 'vscode';
import * as path from 'path';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { ToolsGetSyntaxTreeRequest } from '../langserver/toolsGetSyntaxTreeRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { ToolsGetSyntaxTreeSymbolsRequest } from '../langserver/toolsGetSyntaxTreeSymbolRequest';
import { ToolsCloseSyntaxTreeRequest } from '../langserver/toolsCloseSyntaxTreeRequest';
import { BaseSymbolsWebView } from '../webviews/baseSymbolsWebView';

export class SyntaxTreeView extends BaseSymbolsWebView {
    protected _firstLoad: boolean;

    constructor(devToolsContext : DevToolsExtensionContext, documentUri: vscode.Uri | undefined) {        
        super(devToolsContext, undefined, documentUri);
        this._firstLoad = true;

        this._disposables.push(vscode.workspace.onDidChangeTextDocument(e => {
            if ((e.document) && (this._documentUri) && 
                (e.document.uri.fsPath == this._documentUri.fsPath))
            this.loadSymbols();
        }));

        vscode.window.onDidChangeActiveTextEditor(editor => {
            this.loadSymbols();
        });
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'syntaxtreeview', 'syntaxtreeview.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.SyntaxTreeView';
    }

    protected async loadSymbols() {
        let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
        let source: string = '';

        if (editor)
            source = editor.document.getText();
        
        let request: ToolsGetSyntaxTreeRequest = new ToolsGetSyntaxTreeRequest(source, this._documentUri.fsPath, this._firstLoad);
        this._firstLoad = false;
        let response = await this._devToolsContext.toolsLangServerClient.getSyntaxTree(request);
        if ((response) && (response.root)) {
            let rootSymbol = AZSymbolInformation.fromAny(response.root);
            rootSymbol.updateTree(true, false);            
            this.setSymbols(rootSymbol, undefined);
        } else
            this.setSymbols(undefined, undefined);
    }

    protected async onSymbolSelected(symbolPath: number[]) {
        let request: ToolsGetSyntaxTreeSymbolsRequest = new ToolsGetSyntaxTreeSymbolsRequest(this._documentUri.fsPath, symbolPath);
        let response = await this._devToolsContext.toolsLangServerClient.getSyntaxTreeSymbol(request);
        if (response) {
            this.setSymbolInfo(response.symbol);
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
            this.setSymbolInfo(undefined);
    }

    setSymbolInfo(symbol: any) {
        this.sendMessage({
            command: 'setSymbolInfo',
            data: symbol
        });
    }

    protected onPanelClosed() {
        super.onPanelClosed();
        let request: ToolsCloseSyntaxTreeRequest = new ToolsCloseSyntaxTreeRequest(this._documentUri.fsPath);
        this._devToolsContext.toolsLangServerClient.closeSyntaxTree(request);   
    }
}