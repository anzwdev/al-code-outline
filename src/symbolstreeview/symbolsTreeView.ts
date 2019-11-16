import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from "../webviews/baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { TextEditorHelper } from '../tools/textEditorHelper';

export class SymbolsTreeView extends BaseWebViewEditor {
    protected _devToolsContext : DevToolsExtensionContext;
    private _symbolsChangedHandler : vscode.Disposable | undefined;
    protected _loaded: boolean;
    protected _library: AZSymbolsLibrary | undefined;
    protected _rootSymbol: AZSymbolInformation | undefined;
    protected _documentName: string;
    protected _documentUri: vscode.Uri | undefined;
    protected _selectedSymbolPath: number[] | undefined;
    protected _sourceSymbol : AZSymbolInformation | undefined;
    selectedSymbolRange: vscode.Range | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, documentName: string | undefined, documentUri: vscode.Uri | undefined) {        
        if ((!documentName) && (documentUri))
            documentName = path.parse(documentUri.path).base;
        super(devToolsContext.vscodeExtensionContext, documentName);

        this._documentName = documentName;
        this._documentUri = documentUri;
        this._devToolsContext = devToolsContext;
        this._viewColumn = vscode.ViewColumn.Beside;
        this._loaded = false;
        this._rootSymbol = undefined;
        this._selectedSymbolPath = undefined;
        this._library = undefined;

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
        if (this._documentUri)
            await this.LoadSymbols();
        else
            await this.updateView();
    }

    protected async LoadSymbols() {
        let currDocUri = this._devToolsContext.activeDocumentSymbols.getDocUri();
        if ((currDocUri) && (currDocUri.toString() == this._documentUri.toString()) && (this._devToolsContext.activeDocumentSymbols.rootSymbol)) {
            this._selectedSymbolPath = this._devToolsContext.activeDocumentSymbols.findSymbolPathInSelectionRange(this.selectedSymbolRange);            
            this.setSymbols(this._devToolsContext.activeDocumentSymbols.rootSymbol, this._documentName);            
        } else {
            let library = new AZDocumentSymbolsLibrary(this._devToolsContext, this._documentUri);
            await library.loadAsync(false);            
            this._selectedSymbolPath = library.findSymbolPathInSelectionRange(this.selectedSymbolRange);
            this.setSymbols(library.rootSymbol, this._documentName);
        }
    }

    setSymbols(rootSymbol: AZSymbolInformation | undefined, rootSymbolName: string | undefined) {
        this._sourceSymbol = rootSymbol;

        if (rootSymbol)
            this._rootSymbol = rootSymbol.createCopy(true);
        else
            this._rootSymbol = new AZSymbolInformation();        
        this._rootSymbol.fullName = rootSymbolName;
        
        this._library = new AZSymbolsLibrary();
        if (this._sourceSymbol)
            this._library.setRootSymbol(this._sourceSymbol);
        
        this.updateView();
    }

    onSymbolsChanged(lib: any) {
        if (this._devToolsContext.activeDocumentSymbols.getDocUri().path == this._documentUri.path)
            this.setSymbols(this._devToolsContext.activeDocumentSymbols.rootSymbol, this._documentName);
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

    selectSymbolByRange(range: vscode.Range) {
        if (this._sourceSymbol) {
            let library = new AZDocumentSymbolsLibrary(this._devToolsContext, this._documentUri);
            library.setRootSymbol(this._sourceSymbol);
            let selectedPath : number[] = library.findSymbolPathInSelectionRange(range);

            if (selectedPath) {
                this.sendMessage({
                    command: 'selectSymbol',
                    selected: selectedPath
                });
            }
        }
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;        

        if (message) {
            switch (message.command) {
                case 'definition':
                    this.goToDefinition(message.path);
                    return true;
            }
        }

        return false;
    }

    protected onPanelClosed() {
        if (this._symbolsChangedHandler)
            this._symbolsChangedHandler.dispose();
        if (this._documentUri)
            this._devToolsContext.alSymbolsTreeService.removeUriSymbolsTreeView(this._documentUri);
    }

    protected async findEditor(docUri: vscode.Uri) : Promise<vscode.TextEditor> {
        let docUriString : string = docUri.toString();
        let editors = vscode.window.visibleTextEditors;
        let editorViewColumn: vscode.ViewColumn | undefined;
        for (let i=0; i<editors.length; i++) {
            
            if (editors[i].document) {
                let f1 : string = editors[i].document.uri.toString();
                if (f1 == docUriString) {
                    //return editors[i];
                    editorViewColumn = editors[i].viewColumn;
                }
            }
        }
        
        let targetDoc = await vscode.workspace.openTextDocument(this._documentUri);
        let targetEditor = await vscode.window.showTextDocument(targetDoc, {
            preview : true,
            viewColumn : editorViewColumn  
        });

        return targetEditor;
    }
    
    protected async goToDefinition(nodePath) {
        if (this._library) {
            let symbolList = await this._library.getSymbolsListByPathAsync([nodePath], AZSymbolKind.Undefined);
            if ((symbolList) && (symbolList.length > 0) && (symbolList[0].selectionRange)) {
                TextEditorHelper.openEditor(this._documentUri, true, true, new vscode.Position(symbolList[0].selectionRange.start.line, 
                    symbolList[0].selectionRange.start.character));
            }
        }
    }

    

}