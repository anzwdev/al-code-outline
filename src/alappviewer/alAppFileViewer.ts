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

    private async getHtmlContent(uri: vscode.Uri): Promise<string> {
        var symbolInfo = await this.objectLibraries.getBasicLibrary(uri.fsPath, false);
        var symbolInfoJson = JSON.stringify(symbolInfo);        
        //build page
        var html = this.mainTemplate.replace('##DATA##', symbolInfoJson);
        return html;
    }

    async open() {
        this.htmlContent = await this.getHtmlContent(this.fileUri);
        
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
            if (m)
                this.appFileObjCommand(m.objtype, m.objid, m.cmdname);
        }, null, this.disposables);

    }

    private updateWebView() {
        if (this.panel)
            this.panel.webview.html = this.htmlContent;
    }    

    protected async appFileObjCommand(objType : string, objId : number, commandName : string) {
        if (commandName === 'definition')
            this.objectLibraries.goToDefinition(objType, objId);
        else {
            var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
            if (symbolInfo) {
                if ((commandName === 'selected') && (this.codeOutlineProvider))
                    this.codeOutlineProvider.setAppALSymbolInfo(symbolInfo);
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
            }            
        }

        /*
        if (commandName === 'selected') {
            var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
            if (this.codeOutlineProvider) 
                this.codeOutlineProvider.setAppALSymbolInfo(symbolInfo);
        }
        else if (commandName === 'newcardpage') {
            var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
            let pageBuilder : PageBuilder = new PageBuilder();
            await pageBuilder.showCardPageWizard(symbolInfo);
        }
        else if (commandName === 'newlistpage') {
            var symbolInfo = this.objectLibraries.findALSymbolInfo(objType, objId);
            let pageBuilder : PageBuilder = new PageBuilder();
            await pageBuilder.showListPageWizard(symbolInfo);
        }
        */

    }


}