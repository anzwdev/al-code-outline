'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { ALOutlineProvider } from './aloutline';
import { ObjectBuildersCollection } from './objectbuilders/objectBuildersCollection';
import { ALObjectLibrariesCollection } from './objectlibrary/alObjectLibrariesCollection';
import { ALObjectRunner } from './alObjectRunner';
import { ALAppFileViewer } from './alappviewer/alAppFileViewer';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    //object builders
    const objectBuildersCollection = new ObjectBuildersCollection();
    
    //app preview
    const objectLibraries = new ALObjectLibrariesCollection();
    
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
            'alOutline.groupMembers', 
            () => alOutlineProvider.setGroupMembersMode(true)));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.ungroupMembers', 
            () => alOutlineProvider.setGroupMembersMode(false)));
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

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.viewALApp', 
            (fileUri) => {
                let appViewer : ALAppFileViewer = new ALAppFileViewer(context, objectLibraries, objectBuildersCollection, 
                    alOutlineProvider, objectRunner, fileUri);
                appViewer.open();    
            }));
        

    alOutlineProvider.refresh();
}

// this method is called when your extension is deactivated
export function deactivate() {
}
