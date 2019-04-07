'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { ALObjectRunner } from '../../alObjectRunner';
import { ALAppSymbolsLibrary } from '../../symbollibraries/alAppSymbolsLibrary';
import { ALAppFileViewerData } from './alAppFileViewerData';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { PageBuilder } from '../../objectbuilders/pageBuilder';
import { QueryBuilder } from '../../objectbuilders/queryBuilder';
import { ReportBuilder } from '../../objectbuilders/reportBuilder';
import { XmlPortBuilder } from '../../objectbuilders/xmlPortBuilder';
import { PageExtBuilder } from '../../objectbuilders/pageExtBuilder';
import { TableExtBuilder } from '../../objectbuilders/tableExtBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';

export class ALAppFileViewer {
    private mainTemplate : string;
    private htmlContent : string;
    private panel : vscode.WebviewPanel | undefined;
    private title : string;
    private disposables : vscode.Disposable[] = [];
    private context: DevToolsExtensionContext;
    public symbolsLibrary : ALAppSymbolsLibrary | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, symbolsLib : ALAppSymbolsLibrary) {
        this.symbolsLibrary = symbolsLib;
        this.title = this.symbolsLibrary.displayName;
        if (this.title == "")
            this.title = "AL Object Browser";
        this.context = devToolsContext;

        this.initTemplate();
        this.htmlContent = "";
        this.panel = undefined;        
    }

    dispose() {
        if (this.panel)
            this.panel.dispose();

        while (this.disposables.length) {
            const x = this.disposables.pop();
            if (x)
                x.dispose();
        }
    }

    private initTemplate() {
        var extensionPath = vscode.Uri.file(this.context.vscodeExtensionContext.extensionPath).with({scheme : 'vscode-resource' });
        var fs = require('fs');
        var filePath = this.context.vscodeExtensionContext.asAbsolutePath(path.join('htmlresources', 'objectbrowser', 'objectbrowser.html'));
        var content = fs.readFileSync(filePath, 'utf8');
        this.mainTemplate = content.replace(new RegExp('##PATH##', 'g'), extensionPath);
    }

    private async loadObjects() {
        await this.symbolsLibrary.loadAsync(false);
        let objectData : ALAppFileViewerData = new ALAppFileViewerData(this.symbolsLibrary);
        
        this.panel.webview.postMessage({
            msgtype: 'objectsLoaded',
            data: objectData
        });
    }

    async open() {
        this.htmlContent = this.mainTemplate; //temp await this.getHtmlContent(this.fileUri);
        this.panel = vscode.window.createWebviewPanel("al-code-outline.appViewer", this.title, vscode.ViewColumn.Active, {
            enableScripts : true,
            retainContextWhenHidden: true
        });
       
        this.updateWebView();

        this.panel.onDidDispose(() => this.dispose(), null, this.disposables);
        this.panel.onDidChangeViewState(e => {
            if ((this.panel) && (this.panel.visible)) {
                this.updateWebView();
            }
        }, null, this.disposables);
        this.panel.webview.onDidReceiveMessage(m => {
            if (m) {
                if ((m.command) && (m.command == "documentLoaded")) {
                    this.loadObjects();
                }
                else if (m.command == "execFilterCommand") {
                    this.filterObjects(m.headColumn, m.currentIdFilter, m.currentNameFilter);
                }
                else if (m.command == "errorInFilter") {
                    vscode.window.showErrorMessage('Invalid filter: ' + m.message);
                }
                else if (m.command == "updatePivotObj") {
                    this.updatePivotObjCommand(m.path);
                }
                else if (m.command == "execObjCommand") {
                    this.appFileObjCommand(m.objtype, m.objid, m.path, m.selobj, m.cmdname);
                }
            }                
        }, null, this.disposables);

    }

    private updateWebView() {
        if (this.panel)
            this.panel.webview.html = this.htmlContent;
    }    

    protected findSymbol(obj: any) : AZSymbolInformation | undefined {
        if ((obj.pkind) && (obj.idx)) {
            let symbolList : AZSymbolInformation | undefined = this.symbolsLibrary.rootSymbol.findFirstSymbolByKind(obj.pkind);
            if (symbolList) {
                let idx : number = obj.idx;
                if ((idx >=0) && (idx < symbolList.childSymbols.length))
                    return symbolList.childSymbols[idx];
            }
        }
        return undefined;
    }

    protected async appFileObjCommand(objType : string, objId : number, path: any, selObj: any[], commandName : string) {
        //collect objects
        let selObjList : AZSymbolInformation[] = [];
        if ((selObj) && (selObj.length > 1)) {
            for (let i=0; i<selObj.length; i++) {
                let symbol : AZSymbolInformation | undefined = this.findSymbol(selObj[i]);
                if (symbol)
                    selObjList.push(symbol);
            }
        } else {
            let symbol : AZSymbolInformation | undefined = this.findSymbol(path);
            if (symbol)
                selObjList.push(symbol);
        }

        var multipleObjects = (selObjList.length > 1) && (commandName !== 'runinwebclient');
        if (multipleObjects) {
            if (selObjList.length > 100) {
                let action: string = await vscode.window.showWarningMessage(`You are about to run this command for ${selObjList.length} objects. Do you want to continue?`, {modal: true}, 'Yes', 'No');
                if (action !== 'Yes') {
                    return;
                }
            }
        }

        if (selObjList.length > 0) {
            if (commandName === 'newcardpage')
                await this.createPage(selObjList, 'Card');
            else if (commandName === 'newlistpage')
                await this.createPage(selObjList, 'List');
            else if (commandName === 'newreport')
                await this.createReport(selObjList);
            else if (commandName === 'newxmlport')
                await this.createXmlPort(selObjList);
            else if (commandName === 'newquery')
                await this.createQuery(selObjList);
            else if (commandName === 'extendtable')
                await this.createTableExtension(selObjList);
            else if (commandName === 'extendpage')
                await this.createPageExtension(selObjList);
            else if (commandName === 'runinwebclient')
                ALObjectRunner.runInWebClient(selObjList[0]);
        }
    }

    protected async createPage(objectList : AZSymbolInformation[], pageType : string) {
        let builder = new PageBuilder();
        await builder.showWizard(objectList, pageType);
    }

    protected async createReport(objectList : AZSymbolInformation[]) {
        let builder = new ReportBuilder();
        await builder.showWizard(objectList);
    }

    protected async createQuery(objectList : AZSymbolInformation[]) {
        let builder = new QueryBuilder();
        await builder.showWizard(objectList);
    }

    protected async createXmlPort(objectList : AZSymbolInformation[]) {
        let builder = new XmlPortBuilder();
        await builder.showWizard(objectList);
    }

    protected async createPageExtension(objectList : AZSymbolInformation[]) {
        let builder = new PageExtBuilder();
        await builder.showWizard(objectList);
    }

    protected async createTableExtension(objectList : AZSymbolInformation[]) {
        let builder = new TableExtBuilder();
        await builder.showWizard(objectList);
    }

    protected updatePivotObjCommand(path: any) {
        let rootSymbol : AZSymbolInformation = AZSymbolInformation.create(AZSymbolKind.Document, 'Symbol');
        rootSymbol.addChildItem(this.findSymbol(path));
        this.context.activeDocumentSymbols.setRootSymbol(rootSymbol);
    }

    //#region Filter View

    private async filterObjects(column: string, currentIdFilter: string, currentNameFilter: string) {
        if (column == "Type") {
            const objTypes = ['Table', 'Page', 'Report', 'XmlPort', 'Query', 'Codeunit', 'ControlAddIn', 'PageExtension', 'TableExtension', 'Profile', 'PageCustomization', 'Enum', 'DotNetPackage'];
            const values = await vscode.window.showQuickPick(objTypes, { canPickMany: true, placeHolder: 'Select the object type(s) to filter on.' });
            if (values) {
                this.panel.webview.postMessage({
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
                this.panel.webview.postMessage({
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
                        return 'Valid operators for ID filter are: |, &, =, <>, @, ?, *';
                    } else {
                        return undefined;
                    }
                }
            });
            if (filterExpr !== undefined) {
                this.panel.webview.postMessage({
                    msgtype: 'filterObjects',
                    column: column,
                    filterExpr: filterExpr
                });
            }
        }
    }

    //#endregion

}