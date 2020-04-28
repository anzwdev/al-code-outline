import * as vscode from 'vscode';
import { DocToolsWebView } from "./docToolsWebView";
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { TextEditorHelper } from '../tools/textEditorHelper';

export class BaseSymbolsWebView extends DocToolsWebView {
    protected _sourceSymbolsLibrary: AZSymbolsLibrary;
    protected _sourceRootSymbol: AZSymbolInformation | undefined;    
    protected _rootSymbol: AZSymbolInformation | undefined;
    protected _copySymbols: boolean;
    protected _selectedSymbolPath: number[] | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, documentName: string | undefined, documentUri: vscode.Uri | undefined) {
        super(devToolsContext, documentName, documentUri);
        this._sourceSymbolsLibrary = new AZSymbolsLibrary();
        this._rootSymbol = undefined;
        this._copySymbols = false;
        this._selectedSymbolPath = undefined;
    }

    setSymbols(rootSymbol: AZSymbolInformation | undefined, rootSymbolName: string | undefined) {
        if ((rootSymbol) && (this._copySymbols)) {
            this._rootSymbol = rootSymbol.createCopy(true);
            if (rootSymbolName)
                this._rootSymbol.fullName = rootSymbolName;
        } else
            this._rootSymbol = rootSymbol;
        this._sourceSymbolsLibrary.setRootSymbol(rootSymbol);
        this.updateView();
    }
    
    protected updateView() {
        if (!this._loaded)
            return;
        this.sendMessage({
            command: 'setData',
            data: this._rootSymbol,
            selected: this._selectedSymbolPath
        });
        if (this._rootSymbol)
            this._selectedSymbolPath = undefined;
    }

    protected async onDocumentLoaded() {
        this._loaded = true;
        if (this._documentUri)
            await this.loadSymbols();
        else
            await this.updateView();
    }

    protected async loadSymbols() {

    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;        

        if (message) {
            switch (message.command) {
                case 'definition':
                    if (message.path)
                        this.goToDefinition(message.path);
                    return true;
                case 'symbolselected':
                    if (message.path)
                        this.onSymbolSelected(message.path);
                    return true;
                case 'refresh':
                    this.loadSymbols();
                    return true;
                case 'sync':
                    this.selectSymbolAtCursor();
                    return true;
                }
        }

        return false;
    }

    protected async onSymbolSelected(symbolPath: number[]) {
        this._selectedSymbolPath = symbolPath;
    }

    protected async goToDefinition(nodePath: number[] | undefined) {
        if ((nodePath) && (this._documentUri)) {
            let symbolList = await this._sourceSymbolsLibrary.getSymbolsListByPathAsync([nodePath], AZSymbolKind.Undefined);
            if ((symbolList) && (symbolList.length > 0) && (symbolList[0].selectionRange)) {
                TextEditorHelper.openEditor(this._documentUri, true, true, new vscode.Position(symbolList[0].selectionRange.start.line, 
                    symbolList[0].selectionRange.start.character));
            }
        }
    }

    selectSymbolInRange(range: vscode.Range) {
        this._selectedSymbolPath = this._sourceSymbolsLibrary.findSymbolPathInRange(range);
        if (this._selectedSymbolPath) {
            this.sendMessage({
                command: 'selectSymbol',
                selected: this._selectedSymbolPath
            });
        }
    }

    selectSymbolAtCursor() {
        if (this._documentUri) {
            let editor = TextEditorHelper.findDocumentEditor(this._documentUri);
            if (editor)
                this.selectSymbolInRange(editor.selection);
        }
    }

}