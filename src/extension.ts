'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as vzFileTemplates from 'vz-file-templates';
import { ALOutlineProvider } from './aloutline';
import { ObjectBuildersCollection } from './objectbuilders/objectBuildersCollection';
import { ALObjectLibrariesCollection } from './objectlibrary/alObjectLibrariesCollection';
import { ALObjectRunner } from './alObjectRunner';
import { ALAppFileViewer } from './alappviewer/alAppFileViewer';
import { DevToolsExtensionContext } from './devToolsExtensionContext';
import { ALRunSettingsProcessor } from './filetemplates/alRunSettingsProcessor';
import { ALPageWizard } from './filetemplates/wizards/alPageWizard';
import { ALQueryWizard } from './filetemplates/wizards/alQueryWizard';
import { ALReportWizard } from './filetemplates/wizards/alReportWizard';
import { ALXmlPortWizard } from './filetemplates/wizards/alXmlPortWizard';
import { ALEnumWizard } from './filetemplates/wizards/alEnumWizard';
import { ALEnumExtWizard } from './filetemplates/wizards/alEnumExtWizard';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    const toolsExtensionContext : DevToolsExtensionContext = new DevToolsExtensionContext(context);
    
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
        'alOutline.createCardPage', 
        offset => objectBuildersCollection.pageBuilder.showPageWizard(offset, 'Card')));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createListPage', 
            offset => objectBuildersCollection.pageBuilder.showPageWizard(offset, 'Page')));
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

    //initialize templates
    let filetemplatesExt = vscode.extensions.getExtension('visualzoran.vz-file-templates');    
    if (filetemplatesExt) {
        if (!filetemplatesExt.isActive) {
            filetemplatesExt.activate().then((val) => {
                initializeFileTemplates(toolsExtensionContext, val);
            });
        } else {
            initializeFileTemplates(toolsExtensionContext, filetemplatesExt.exports);
        }
    }

    return toolsExtensionContext;
}

function initializeFileTemplates(toolsExtensionContext : DevToolsExtensionContext, api : vzFileTemplates.IVZFileTemplatesApi) {
    
    //register wizards
    api.registerWizard(new ALPageWizard(toolsExtensionContext));
    api.registerWizard(new ALQueryWizard(toolsExtensionContext));
    api.registerWizard(new ALReportWizard(toolsExtensionContext));
    api.registerWizard(new ALXmlPortWizard(toolsExtensionContext));
    api.registerWizard(new ALEnumWizard(toolsExtensionContext));
    api.registerWizard(new ALEnumExtWizard(toolsExtensionContext));
    
    //register templates folders
    api.registerTemplatesFolder(toolsExtensionContext.vscodeExtensionContext.asAbsolutePath('templates'));

    //register variables processor
    api.registerRunSettingsProcessor(new ALRunSettingsProcessor());
}

// this method is called when your extension is deactivated
export function deactivate() {
}
