import * as vscode from 'vscode';
import * as path from 'path';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { ToolsGetSyntaxTreeRequest } from '../langserver/toolsGetSyntaxTreeRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { ToolsGetSyntaxTreeSymbolsRequest } from '../langserver/toolsGetSyntaxTreeSymbolRequest';
import { ToolsCloseSyntaxTreeRequest } from '../langserver/toolsCloseSyntaxTreeRequest';
import { BaseSymbolsWebView } from '../webviews/baseSymbolsWebView';
import { SyntaxTreeViewMode } from './syntaxTreeViewMode';
import { ToolsGetSyntaxTreeResponse } from '../langserver/toolsGetSyntaxTreeResponse';

export class SyntaxTreeView extends BaseSymbolsWebView {
    protected _firstLoad: boolean;
    protected _viewMode: SyntaxTreeViewMode;

    constructor(devToolsContext : DevToolsExtensionContext, documentUri: vscode.Uri | undefined) {        
        super(devToolsContext, undefined, documentUri);
        this._firstLoad = true;
        this._viewMode = SyntaxTreeViewMode.ClassView;

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

    protected sendSetDataMessage() {
        this.sendMessage({
            command: 'setData',
            data: this._rootSymbol,
            viewMode: this._viewMode,
            selected: this._selectedSymbolPath
        });
    }

    protected async loadSymbols() {
        if (!this._documentUri) {
            return;
        }
        let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
        let source: string = '';
        let projectPath: string | undefined = undefined;

        if (editor) {
            source = editor.document.getText();
            projectPath = editor.document.uri.fsPath;
        }

        let request: ToolsGetSyntaxTreeRequest = new ToolsGetSyntaxTreeRequest(source, this._documentUri.fsPath, projectPath, this._firstLoad);
        this._firstLoad = false;
        let response: ToolsGetSyntaxTreeResponse | undefined = undefined;
        switch (this._viewMode) {
            case SyntaxTreeViewMode.TreeView:
                response = await this._devToolsContext.toolsLangServerClient.getSyntaxTree(request);
                break;
            case SyntaxTreeViewMode.ClassView:
                response = await this._devToolsContext.toolsLangServerClient.getRawSyntaxTree(request);
                break;
        }
        
        if ((response) && (response.root)) {
            let rootSymbol = AZSymbolInformation.fromAny(response.root);
            rootSymbol.updateTree(true, false);            
            this.setSymbols(rootSymbol, undefined);
        } else
            this.setSymbols(undefined, undefined);
    }

    protected async onSymbolSelected(symbolPath: number[]) {
        if (!this._documentUri)
            return;

        let request: ToolsGetSyntaxTreeSymbolsRequest = new ToolsGetSyntaxTreeSymbolsRequest(this._documentUri.fsPath, symbolPath);
        let response = await this._devToolsContext.toolsLangServerClient.getRawSyntaxTreeSymbol(request);
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
        if (this._documentUri) {
            let request: ToolsCloseSyntaxTreeRequest = new ToolsCloseSyntaxTreeRequest(this._documentUri.fsPath);
            this._devToolsContext.toolsLangServerClient.closeRawSyntaxTree(request);   
        }
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;        
        }

        if (message) {
            switch (message.command) {
                case 'treeview':
                    this.setView(SyntaxTreeViewMode.TreeView);
                    return true;
                case 'classview':
                    this.setView(SyntaxTreeViewMode.ClassView);
                    return true;
                }
        }

        return false;
    }

    protected setView(newViewMode: SyntaxTreeViewMode) {
        if (this._viewMode !== newViewMode) {
            this._viewMode = newViewMode;
            this.loadSymbols();
        }
    }


}