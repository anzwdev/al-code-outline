'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { ALObjectLibrariesCollection } from '../objectlibrary/alObjectLibrariesCollection';
import { ALOutlineProvider } from '../alOutline';
import { PageBuilder } from '../objectbuilders/pageBuilder';
import { ObjectBuildersCollection } from '../objectbuilders/objectBuildersCollection';
import { ALObjectRunner } from '../alObjectRunner';

export class ALAppFileViewer {
    private objectLibraries : ALObjectLibrariesCollection;
    private mainTemplate : string;
    private codeOutlineProvider : ALOutlineProvider;
    private objectBuilders : ObjectBuildersCollection;
    private alObjectRunner : ALObjectRunner;
    private htmlContent : string;
    private panel : vscode.WebviewPanel | undefined;
    private title : string;
    private disposables : vscode.Disposable[] = [];
    private fileUri : vscode.Uri;

    constructor(context : vscode.ExtensionContext, objLibraries : ALObjectLibrariesCollection, objBuilders : ObjectBuildersCollection,
        outlineProvider : ALOutlineProvider, objectRunner : ALObjectRunner, uri : vscode.Uri) {
        this.objectLibraries = objLibraries;
        this.objectBuilders = objBuilders;
        this.codeOutlineProvider = outlineProvider;
        this.alObjectRunner = objectRunner;
        this.fileUri = uri;
        this.initTemplate(context);
        this.htmlContent = "";
        this.title = path.parse(uri.fsPath).base;
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

    private initTemplate(context: vscode.ExtensionContext) {
        var extensionPath = vscode.Uri.file(context.extensionPath).with({scheme : 'vscode-resource' });
        var fs = require('fs');
        var filePath = context.asAbsolutePath(path.join('htmlresources', 'objectbrowser', 'objectbrowser.html'));
        var content = fs.readFileSync(filePath, 'utf8');
        this.mainTemplate = content.replace(new RegExp('##PATH##', 'g'), extensionPath);
    }

    private async loadObjects(uri: vscode.Uri) {
        var symbolInfo = await this.objectLibraries.getBasicLibrary(uri.fsPath, false);
        if (symbolInfo)
            this.panel.webview.postMessage({
                msgtype: 'objectsLoaded',
                data: symbolInfo
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
                    this.loadObjects(this.fileUri);
                }
                else if (m.command == "execFilterCommand") {
                    this.filterObjects(m.headColumn, m.currentIdFilter, m.currentNameFilter);
                }
                else if (m.command == "errorInFilter") {
                    vscode.window.showErrorMessage('Invalid filter: ' + m.message);
                }
                else if (m.command == "updatePivotObj") {
                    this.updatePivotObjCommand(m.objtype, m.objid);
                }
                else if (m.command == "execObjCommand") {
                    this.appFileObjCommand(m.objtype, m.objid, m.selobjids, m.cmdname);
                }
            }                
        }, null, this.disposables);

    }

    private updateWebView() {
        if (this.panel)
            this.panel.webview.html = this.htmlContent;
    }    

    protected async appFileObjCommand(objType : string, objId : number, selObjIds: number[], commandName : string) {
        var multipleObjects = (selObjIds.length > 1) && (commandName !== 'runinwebclient');
        if (multipleObjects) {
            if (selObjIds.length > 100) {
                let action: string = await vscode.window.showWarningMessage(`You are about to run this command for ${selObjIds.length} objects. Do you want to continue?`, {modal: true}, 'Yes', 'No');
                if (action !== 'Yes') {
                    return;
                }
            }

            var symbolInfoList = this.objectLibraries.findALSymbolInfoList(objType, selObjIds);
            if (symbolInfoList) {
                if (commandName === 'newcardpage')
                    await this.objectBuilders.pageBuilder.showMultiPageWizard(symbolInfoList, 'Card');
                else if (commandName === 'newlistpage')
                    await this.objectBuilders.pageBuilder.showMultiPageWizard(symbolInfoList, 'List');
                else if (commandName === 'newreport')
                    await this.objectBuilders.reportBuilder.showMultiReportWizard(symbolInfoList);
                else if (commandName === 'newxmlport')
                    await this.objectBuilders.xmlPortBuilder.showMultiXmlPortWizard(symbolInfoList);
                else if (commandName === 'newquery')
                    await this.objectBuilders.queryBuilder.showMultiQueryWizard(symbolInfoList);
                else if (commandName === 'extendtable')
                    await this.objectBuilders.tableExtBuilder.showMultiTableExtWizard(symbolInfoList);
                else if (commandName === 'extendpage')
                    await this.objectBuilders.pageExtBuilder.showMultiPageExtWizard(symbolInfoList);
            }
        }
        else {
            var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
            if (symbolInfo) {
                if (commandName === 'newcardpage')
                    await this.objectBuilders.pageBuilder.showPageWizard(symbolInfo, 'Card');
                else if (commandName === 'newlistpage')
                    await this.objectBuilders.pageBuilder.showPageWizard(symbolInfo, 'List');
                else if (commandName === 'newreport')
                    await this.objectBuilders.reportBuilder.showReportWizard(symbolInfo);
                else if (commandName === 'newxmlport')
                    await this.objectBuilders.xmlPortBuilder.showXmlPortWizard(symbolInfo);
                else if (commandName === 'newquery')
                    await this.objectBuilders.queryBuilder.showQueryWizard(symbolInfo);
                else if (commandName === 'runinwebclient')
                    this.alObjectRunner.runInWebClient(symbolInfo);
                else if (commandName === 'extendtable')
                    await this.objectBuilders.tableExtBuilder.showTableExtWizard(symbolInfo);
                else if (commandName === 'extendpage')
                    await this.objectBuilders.pageExtBuilder.showPageExtWizard(symbolInfo);
            }   
        }         
    }

    protected async updatePivotObjCommand(objType: string, objId: number) {
        var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
        if (symbolInfo && this.codeOutlineProvider) {
            this.codeOutlineProvider.setAppALSymbolInfo(symbolInfo);
        }
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