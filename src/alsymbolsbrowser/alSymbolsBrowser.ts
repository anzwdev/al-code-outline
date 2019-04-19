'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from '../webviews/baseWebViewEditor';
import { AZSymbolsLibrary } from '../symbolLibraries/azSymbolsLibrary';
import { ALObjectRunner } from '../alObjectRunner';
import { ALSyntaxHelper } from '../allanguage/alSyntaxHelper';
import { ALLangServerProxy } from '../allanguage/alLangServerProxy';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { PageBuilder } from '../objectbuilders/pageBuilder';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALAppFileViewer } from '../alappviewer/alAppFileViewer';

export class ALSymbolsBrowser extends BaseWebViewEditor {
    protected _library : AZSymbolsLibrary;
    protected _selectedObject : AZSymbolInformation | undefined;
    protected _searchText : string;
    protected _searchResult : AZSymbolInformation | undefined;
    protected _showObjectIds : boolean;
    protected _devToolsContext : DevToolsExtensionContext;

    constructor(devToolsContext : DevToolsExtensionContext,  library : AZSymbolsLibrary) {
        let name : string = library.displayName;
        if (name == '')
            name =  'AL Object Browser';
        super(devToolsContext.vscodeExtensionContext, library.displayName);
        this._devToolsContext = devToolsContext;
        this._searchText = '';
        this._searchResult = undefined;
        this._library = library;
        this._selectedObject = undefined;
        this._showObjectIds = false;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alsymbolsbrowser', 'symbolsbrowser.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.ALObjectBrowser';
    }

    protected async onDocumentLoaded() {
        //load library
        await this._library.loadAsync(false);

        //send data to the web view
        this.updateObjects();
    }

    protected updateObjects() {        
        let data : AZSymbolInformation | undefined = undefined;
        if (this._searchResult)
            data = this._searchResult;
        else if (this._searchText == '')
            data = this._library.objectListRootSymbol;

        this.sendMessage({
            command : 'setData',
            data : data
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'showlist':
                this.showListView();
                break;
            case 'objselected':
                this.onObjectSelected(message.path);
                return true;
            case 'search':
                this.onSearch(message.text);
                return true;
            case 'definition':
                this.goToDefinition(message.path);
                return true;
            case 'runinwebclient':
                this.runInWebClient(message.path);
                return true;
            case 'newcardpage':
                this.createPage(message.path, message.selpaths, "Card");
                return true;
            case 'newlistpage':
                this.createPage(message.path, message.selpaths, "List");
                return true;
            case 'newreport':
                this.createReport(message.path, message.selpaths);
                return true;
            case 'newxmlport':
                this.createXmlPort(message.path, message.selpaths);
                return true;
            case 'newquery':
                this.createQuery(message.path, message.selpaths);
                return true;
//            case 'extendtable':
//                this.createTableExt(message.path, message.selpaths);
//                return true;
//            case 'extendpage':
//                this.createPageExt(message.path, message.selpaths);
//                return true;
        }

        return false;
    }

    protected async getObjectsFromPath(selPaths: number[][] | undefined, kind : AZSymbolKind) : Promise<AZSymbolInformation[] | undefined> {
        if (!selPaths)
            return undefined;

        let objList = await this._library.getSymbolListByPath(selPaths, kind);
        if (!objList)
            return undefined;

        if (objList.length > 100) {
            let action: string = await vscode.window.showWarningMessage(`You are about to run this command for ${selPaths.length} objects. Do you want to continue?`, {modal: true}, 'Yes', 'No');
            if (action !== 'Yes') {
                return undefined;
            }
        }

        return objList;
    }

    protected async createPage(path : number[] | undefined, selPaths: number[][] | undefined, pageType : string) {
        let symbolList = await this.getObjectsFromPath(selPaths, AZSymbolKind.TableObject);
        if (symbolList) {
            let builder : PageBuilder = new PageBuilder();
            await builder.showWizard(symbolList, pageType);
        }
    }

    protected createQuery(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (symbol) {
            //!!!let builder : ALQueryDocumentBuilder = new ALQueryDocumentBuilder();
            //!!!builder.buildFromSymbolAsync(symbol);
        }
    }

    protected createReport(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (symbol) {
            //!!!let builder : ALReportDocumentBuilder = new ALReportDocumentBuilder();
            //!!!builder.buildFromSymbolAsync(symbol);
        }
    }

    protected createXmlPort(path : number[] | undefined, selPaths: number[][] | undefined) {
        let symbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (symbol) {
            //!!!let builder : ALXmlPortDocumentBuilder = new ALXmlPortDocumentBuilder();
            //!!!builder.buildFromSymbolAsync(symbol);
        }
    }

    protected onObjectSelected(path : number[] | undefined) {       
        this._selectedObject = this._library.getObjectSymbolByPath(path);
        if (this._selectedObject)
            this.sendMessage({
                command: 'setSelObjData',
                data: this._selectedObject
            });
    }

    protected onSearch(text : string) {
        this._searchText = text;
        this._searchResult = this._library.search(text);
        this.updateObjects();
    }

    protected async goToDefinition(path : number[] | undefined) {
        let alSymbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (alSymbol) {
            //get data type name
            let typeName : string | undefined = ALSyntaxHelper.kindToVariableType(alSymbol.kind);
            if (!typeName) {
                vscode.window.showErrorMessage('This object type is not supported.');
                return;
            }

            let langProxy : ALLangServerProxy = new ALLangServerProxy();
            let targetLocation : vscode.Location | undefined = await langProxy.getDefinitionLocation(typeName, alSymbol.name);
    
            if (targetLocation) {
                try {
                    //open document
                    let targetDoc = await vscode.workspace.openTextDocument(targetLocation.uri);
                    let targetEditor = await vscode.window.showTextDocument(targetDoc, {
                        preview : true
                    });

                    //go to location
                    targetEditor.selection = new vscode.Selection(targetLocation.range.start, targetLocation.range.start);
                }
                catch (e) {
                    vscode.window.showErrorMessage(e.message);
                }
            }
        }
    }

    protected runInWebClient(path : number[] | undefined) {
        let alSymbol : AZSymbolInformation | undefined = this._library.getObjectSymbolByPath(path);
        if (alSymbol) {
            ALObjectRunner.runInWebClient(alSymbol);
        }
    }

    protected showListView() {
        this._devToolsContext.setUseSymbolsBrowser(false);
        this._panel.dispose();
        let appViewer : ALAppFileViewer = new ALAppFileViewer(this._devToolsContext, this._library);
        appViewer.open();
    }

} 
