'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { ALBaseSymbolsBrowser } from "./alBaseSymbolsBrowser";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolsLibrary } from "../symbollibraries/azSymbolsLibrary";
import { ALSymbolsBrowser } from './alSymbolsBrowser';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALObjectBrowserItem } from './alObjectBrowserItem';

/**
 * AL Objects Browser
 * allows to browse AL objects in a flat list like in the Object Designer in Dynamics Nav
 */
export class ALObjectsBrowser extends ALBaseSymbolsBrowser {
    protected _itemsList: ALObjectBrowserItem[];
    protected _showLibraries: boolean;
    
    constructor(devToolsContext : DevToolsExtensionContext,  library : AZSymbolsLibrary) {
        super(devToolsContext, library);
        this._itemsList = [];
        this._showLibraries = false;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'objectbrowser', 'objectbrowser.html');
    }

    protected getViewType() : string {
        return 'azALDevTools.ALObjectsBrowser';
    }

    protected async onDocumentLoaded() {
        //load library
        await this._library.loadAsync(false);
        //let objectData : ALObjectsBrowserData = new ALObjectsBrowserData(this._library);

        this._itemsList = [];
        this._showLibraries = false;
        if (this._library.rootSymbol)
            this.collectSymbols(this._library.rootSymbol, '');

        //send data to the web view
        this.sendMessage({
            command: 'setData',
            data: this._itemsList,
            showLibraries: this._showLibraries
        });
    }

    protected collectSymbols(symbol: AZSymbolInformation, libraryName: string) {
        if (symbol.isALObject()) {
            this._itemsList.push(new ALObjectBrowserItem(symbol.kind, symbol.id, symbol.name, libraryName, symbol.getPath()))
        } else if (symbol.childSymbols) {
            if (symbol.kind == AZSymbolKind.Package) {
                libraryName = symbol.name;
                if ((!this._showLibraries) && (this._itemsList.length > 0))
                    this._showLibraries = true;
            }

            for (let i=0; i<symbol.childSymbols.length; i++) {
                symbol.childSymbols[i].parent = symbol;
                this.collectSymbols(symbol.childSymbols[i], libraryName);
            }
        }
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'showTreeView':
                this.showTreeView();
                break;
            case 'currRowChanged':
                this.updatePivotObjCommand(message.path);
                return true;
        }

        return false;
    }

    protected async updatePivotObjCommand(symbolPath: number[] | undefined) {
        let rootSymbol : AZSymbolInformation = AZSymbolInformation.create(AZSymbolKind.Document, 'Symbol');
        if ((symbolPath) && (symbolPath.length > 0)) {
            let pathList: number[][] = [symbolPath];
            let symbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync(pathList, AZSymbolKind.AnyALObject);
            if ((symbolList) && (symbolList.length > 0))        
                rootSymbol.addChildItem(symbolList[0]);
        }
        this._devToolsContext.activeDocumentSymbols.setRootSymbol(rootSymbol);
    }

    protected showTreeView() {
        this._devToolsContext.setUseSymbolsBrowser(true);
        if (this._panel)
            this._panel.dispose();
        let objectBrowser : ALSymbolsBrowser = new ALSymbolsBrowser(this._devToolsContext, this._library);
        objectBrowser.show();
    }

    //#region Filter View

    private async filterObjects(column: string, currentIdFilter: string, currentNameFilter: string) {
        if (column == "Type") {
            const objTypes = ['Table', 'Page', 'Report', 'XmlPort', 'Query', 'Codeunit', 'ControlAddIn', 'PageExtension', 'TableExtension', 'Profile', 'PageCustomization', 'Enum', 'DotNetPackage'];
            const values = await vscode.window.showQuickPick(objTypes, { canPickMany: true, placeHolder: 'Select the object type(s) to filter on.' });
            if (values) {
                this.sendMessage({
                    msgtype: 'filterObjects',
                    column: column,
                    filterSet: values
                });
            }
        }
        else if (column == "ID") {
            let regexp = new RegExp('^\s*|(((((=?)|(<>)|(>)|(>=)|(<)|(<=)|(\\.\\.))\s*[0-9]+)|([0-9]+\s*\\.\\.\s*[0-9]*))(\s*((\\|)|(&))\s*(((=?)|(<>)|(>)|(>=)|(<)|(<=)|(\\.\\.))\s*[0-9]+\s*)|([0-9]+\s*\\.\\.\s*[0-9]*))*)$');
            const filterExpr = await vscode.window.showInputBox({
                value  : currentIdFilter,
                prompt : 'Please enter an ID filter expression (e.g., "10..20|>=50").',
                ignoreFocusOut: true,
                validateInput: (text: string): string | undefined => {
                    if (!regexp.test(text)) {
                        return 'Valid operators for ID filter are: |, &, =, <>, >, >=, <, <=, ..';
                    } else {
                        return undefined;
                    }
                }
            });
            if (filterExpr !== undefined) {
                this.sendMessage({
                    msgtype: 'filterObjects',
                    column: column,
                    filterExpr: filterExpr
                });
            }
        }
        else if (column == "Name") {
            let regexp = new RegExp('^\s*|((@?((=?)|(<>))[^\(\)=<>&\|@]+)(\s*((\\|)|(&))\s*(@?((=?)|(<>))[^\(\)=<>&\|@]+))*)$');
            const filterExpr = await vscode.window.showInputBox({
                value  : currentNameFilter,
                prompt : 'Please enter a filter expression (e.g., "<>@Item*").',
                ignoreFocusOut: true,
                validateInput: (text: string): string | undefined => {
                    if (!regexp.test(text)) {
                        return 'Valid operators for Name filter are: |, &, =, <>, @, ?, *';
                    } else {
                        return undefined;
                    }
                }
            });
            if (filterExpr !== undefined) {
                this.sendMessage({
                    msgtype: 'filterObjects',
                    column: column,
                    filterExpr: filterExpr
                });
            }
        }
        else if (column == "Package") {
            let regexp = new RegExp('^\s*|((@?((=?)|(<>))[^\(\)=<>&\|@]+)(\s*((\\|)|(&))\s*(@?((=?)|(<>))[^\(\)=<>&\|@]+))*)$');
            const filterExpr = await vscode.window.showInputBox({
                value  : currentNameFilter,
                prompt : 'Please enter a filter expression (e.g., "<>@Application*").',
                ignoreFocusOut: true,
                validateInput: (text: string): string | undefined => {
                    if (!regexp.test(text)) {
                        return 'Valid operators for Package filter are: |, &, =, <>, @, ?, *';
                    } else {
                        return undefined;
                    }
                }
            });
            if (filterExpr !== undefined) {
                this.sendMessage({
                    msgtype: 'filterObjects',
                    column: column,
                    filterExpr: filterExpr
                });
            }
        }
    }

    //#endregion


}