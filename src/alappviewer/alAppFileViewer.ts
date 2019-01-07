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
            enableScripts : true
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
                    this.filterObjects(m.headColumn, m.currentIdFilter);
                }
                else if (m.command == "errorInFilter") {
                    vscode.window.showErrorMessage('Invalid filter: ' + m.message);
                }
                else {
                    this.appFileObjCommand(m.objtype, m.objid, m.cmdname);
                }
            }                
        }, null, this.disposables);

    }

    private updateWebView() {
        if (this.panel)
            this.panel.webview.html = this.htmlContent;
    }    

    protected async appFileObjCommand(objType : string, objId : number, commandName : string) {
        var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
        if (symbolInfo) {
            if ((commandName === 'selected') && (this.codeOutlineProvider)) {
                this.codeOutlineProvider.setAppALSymbolInfo(symbolInfo);
            }
            else if (commandName === 'newcardpage')
                await this.objectBuilders.pageBuilder.showCardPageWizard(symbolInfo);
            else if (commandName === 'newlistpage')
                await this.objectBuilders.pageBuilder.showListPageWizard(symbolInfo);
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

    //#region Filter View

    private async filterObjects(column: string, currentIdFilter: string) {
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
            //TODO: Do something with Name filtering
            vscode.window.showErrorMessage('Filtering not implemented yet for the "Name" column!');
        }
    }

    //#endregion

}