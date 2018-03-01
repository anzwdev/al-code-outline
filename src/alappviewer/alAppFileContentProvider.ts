'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { ALObjectLibrariesCollection } from '../objectlibrary/alObjectLibrariesCollection';
import { ALOutlineProvider } from '../alOutline';
import { PageBuilder } from '../objectbuilders/pageBuilder';
import { ObjectBuildersCollection } from '../objectbuilders/objectBuildersCollection';

export class ALAppFileContentProvider implements vscode.TextDocumentContentProvider {
    onDidChange?: vscode.Event<vscode.Uri>;

    private objectLibraries : ALObjectLibrariesCollection;
    private mainTemplate : string;
    private codeOutlineProvider : ALOutlineProvider;
    private objectBuilders : ObjectBuildersCollection;

    constructor(context : vscode.ExtensionContext, objLibraries : ALObjectLibrariesCollection, objBuilders : ObjectBuildersCollection) {
        this.objectLibraries = objLibraries;
        this.objectBuilders = objBuilders;
        this.initTemplate(context);
    }

    setOutlineProvider(newValue : ALOutlineProvider) {
        this.codeOutlineProvider = newValue;
    }

    private initTemplate(context: vscode.ExtensionContext) {
        var fs = require('fs');
        var filePath = context.asAbsolutePath(path.join('htmlresources', 'objectbrowser', 'objectbrowser.html'));
        var content = fs.readFileSync(filePath, 'utf8');
        this.mainTemplate = content.replace(new RegExp('##PATH##', 'g'), context.asAbsolutePath('.'));
    }

    provideTextDocumentContent(uri: vscode.Uri, token: vscode.CancellationToken): vscode.ProviderResult<string> {
        var symbolInfo = this.objectLibraries.getBasicLibrary(uri.fsPath, false);
        var symbolInfoJson = JSON.stringify(symbolInfo);
        //build page
        var html = this.mainTemplate.replace('##DATA##', symbolInfoJson);
        return html;
    }

    async appFileObjCommand(objType : string, objId : number, commandName : string) {
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