'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { ALOutlineProvider } from './aloutline';
import { ObjectBuildersCollection } from './objectbuilders/objectBuildersCollection';
import { ALObjectLibrariesCollection } from './objectlibrary/alObjectLibrariesCollection';
import { ALAppFileContentProvider } from './alappviewer/alAppFileContentProvider';
import { ALObjectRunner } from './alObjectRunner';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    //object builders
    const objectBuildersCollection = new ObjectBuildersCollection();
    
    //app preview
    const objectLibraries = new ALObjectLibrariesCollection();
    const alAppFileContentProvider = new ALAppFileContentProvider(context, objectLibraries, objectBuildersCollection);
    const alAppRegistration = vscode.workspace.registerTextDocumentContentProvider('alOutline.appViewer', alAppFileContentProvider);
    
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.appFileObjCommand', 
            (objtype : string, objid : number, cmdname : string) => {
                alAppFileContentProvider.appFileObjCommand(objtype, objid, cmdname);
            }));

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.viewALApp', 
            (fileUri) => {
                let appUri = fileUri.with( {scheme: 'alOutline.appViewer'});
                vscode.commands.executeCommand('vscode.previewHtml', appUri, vscode.ViewColumn.Active, 'AL Object Browser');
                //return vscode.commands.executeCommand('vscode.previewHtml', previewUri, vscode.ViewColumn.Two, 'CSS Property Preview').then((success) => {
                //}, (reason) => {
                //		vscode.window.showErrorMessage(reason);
                //});
            }));

    //al code outline
    const alOutlineProvider = new ALOutlineProvider(context);

    context.subscriptions.push(
        vscode.window.registerTreeDataProvider('alOutline', alOutlineProvider));

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.refresh', 
            () => alOutlineProvider.refresh()));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createCardPage', 
            offset => objectBuildersCollection.pageBuilder.showCardPageWizard(offset)));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createListPage', 
            offset => objectBuildersCollection.pageBuilder.showListPageWizard(offset)));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createReport', 
            offset => objectBuildersCollection.reportBuilder.showReportWizard(offset)));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createXmlPort', 
            offset => objectBuildersCollection.xmlPortBuilder.showXmlPortWizard(offset)));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createQuery', 
            offset => objectBuildersCollection.queryBuilder.showQueryWizard(offset)));
    
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.openSelection', range => {
                alOutlineProvider.select(range);
            }));

    //object runner
    const objectRunner = new ALObjectRunner(alOutlineProvider);

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.runPage', offset => {
                objectRunner.runInWebClient(offset);
            }));

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.runTable', 
            offset => {
                objectRunner.runInWebClient(offset);
            }));

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.runReport', 
            offset => {
                objectRunner.runInWebClient(offset);
            }));

    alAppFileContentProvider.setOutlineProvider(alOutlineProvider);
    alAppFileContentProvider.setALObjectRunner(objectRunner);

    alOutlineProvider.refresh();
}

// this method is called when your extension is deactivated
export function deactivate() {
}
