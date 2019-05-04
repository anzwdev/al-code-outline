'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { ALBaseSymbolsBrowser } from "./alBaseSymbolsBrowser";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolsLibrary } from "../symbollibraries/azSymbolsLibrary";
import { ALSymbolsBrowser } from './alSymbolsBrowser';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALObjectsBrowserData } from './alObjectsBrowserData';

/**
 * AL Objects Browser
 * allows to browse AL objects in a flat list like in the Object Designer in Dynamics Nav
 */
export class ALObjectsBrowser extends ALBaseSymbolsBrowser {
    
    constructor(devToolsContext : DevToolsExtensionContext,  library : AZSymbolsLibrary) {
        super(devToolsContext, library);
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
        let objectData : ALObjectsBrowserData = new ALObjectsBrowserData(this._library);

        //send data to the web view
        this.sendMessage({
            msgtype: 'objectsLoaded',
            data: objectData
        });
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'showTreeView':
                this.showTreeView();
                break;
            case 'execFilterCommand':
                this.filterObjects(message.headColumn, message.currentIdFilter, message.currentNameFilter);
                return true;
            case 'errorInFilter':
                vscode.window.showErrorMessage('Invalid filter: ' + message.message);
                return true;
            case 'updatePivotObj':
                this.updatePivotObjCommand(message.path);
                return true;
        }

        return false;
    }

    protected async updatePivotObjCommand(path: any) {
        let rootSymbol : AZSymbolInformation = AZSymbolInformation.create(AZSymbolKind.Document, 'Symbol');
        let symbolList : AZSymbolInformation[] | undefined = await this._library.getSymbolsListByPathAsync([path], AZSymbolKind.AnyALObject);
        if ((symbolList) && (symbolList.length > 0))        
            rootSymbol.addChildItem(symbolList[0]);
        this._devToolsContext.activeDocumentSymbols.setRootSymbol(rootSymbol);
    }

    protected showTreeView() {
        this._devToolsContext.setUseSymbolsBrowser(true);
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