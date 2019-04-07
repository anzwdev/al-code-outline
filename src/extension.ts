'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as vzFileTemplates from 'vz-file-templates';
import { ALObjectRunner } from './alObjectRunner';
import { DevToolsExtensionContext } from './devToolsExtensionContext';
import { ALRunSettingsProcessor } from './filetemplates/alRunSettingsProcessor';
import { ALPageWizard } from './filetemplates/wizards/alPageWizard';
import { ALQueryWizard } from './filetemplates/wizards/alQueryWizard';
import { ALReportWizard } from './filetemplates/wizards/alReportWizard';
import { ALXmlPortWizard } from './filetemplates/wizards/alXmlPortWizard';
import { ALEnumWizard } from './filetemplates/wizards/alEnumWizard';
import { ALEnumExtWizard } from './filetemplates/wizards/alEnumExtWizard';
import { SymbolsTreeProvider } from './outlineview/symbolsTreeProvider';
import { ALAppSymbolsLibrary } from './symbollibraries/alAppSymbolsLibrary';
import { ALSymbolsBrowser } from './alsymbolsbrowser/alSymbolsBrowser';
import { ALActionImageBrowser } from './actionimagebrowser/alActionImageBrowser';
import { ALAppFileViewer } from './obsolete/alappviewer/alAppFileViewer';
import { ALAppSymbolsLibrariesCache } from './symbollibraries/alAppSymbolsLibrariesCache';
import { PageBuilder } from './objectbuilders/pageBuilder';
import { ReportBuilder } from './objectbuilders/reportBuilder';
import { XmlPortBuilder } from './objectbuilders/xmlPortBuilder';
import { QueryBuilder } from './objectbuilders/queryBuilder';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    const toolsExtensionContext : DevToolsExtensionContext = new DevToolsExtensionContext(context);
    const symbolsTreeProvider : SymbolsTreeProvider = new SymbolsTreeProvider(toolsExtensionContext);
    const librariesCache : ALAppSymbolsLibrariesCache = new ALAppSymbolsLibrariesCache(toolsExtensionContext);
    
	context.subscriptions.push(toolsExtensionContext);

    //document symbols tree provider
    context.subscriptions.push(
        vscode.window.registerTreeDataProvider('azALDevTools_SymbolsTreeProvider', symbolsTreeProvider));

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.selectDocumentText',
            (range) => {
                if (vscode.window.activeTextEditor) {
                    let vscodeRange = new vscode.Range(range.start.line, range.start.character, 
                        range.end.line, range.end.character);

                    vscode.window.activeTextEditor.revealRange(vscodeRange, vscode.TextEditorRevealType.Default);
                    vscode.window.activeTextEditor.selection = new vscode.Selection(vscodeRange.start, vscodeRange.end);
                    vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');            
                }
    }));

    //al app viewer
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.viewALApp', 
            (fileUri) => {
                let uri : vscode.Uri = fileUri;
                let lib : ALAppSymbolsLibrary = librariesCache.getOrCreate(uri.fsPath);

                let useNewViewer = vscode.workspace.getConfiguration('alOutline').get('enableFeaturePreview');
                if (useNewViewer) {
                    let objectBrowser : ALSymbolsBrowser = new ALSymbolsBrowser(context, lib);
                    objectBrowser.show();
                } else {
                    let appViewer : ALAppFileViewer = new ALAppFileViewer(toolsExtensionContext, lib);
                    appViewer.open();    
                }
    }));

    //al action images viewer
    context.subscriptions.push(
        vscode.commands.registerCommand(
           'azALDevTools.viewActionImages',
            () => {
                let actionImageBrowser : ALActionImageBrowser = new ALActionImageBrowser(context);
                actionImageBrowser.show();
            }
        )
    );

    //----------------------------------
    //Outline context menu commands
    //----------------------------------
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.refreshOutlineView', 
            () => symbolsTreeProvider.refresh()));

    //al symbols commands
    context.subscriptions.push(
        vscode.commands.registerCommand(
        'alOutline.createCardPage', 
        offset => {
            let builder = new PageBuilder();
            builder.showPageWizard(offset, 'Card');
        }));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createListPage', 
            offset => {
                let builder = new PageBuilder();
                builder.showPageWizard(offset, 'List');
        }));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createReport', 
            offset => {
                let builder = new ReportBuilder();
                builder.showReportWizard(offset);
        }));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createXmlPort', 
            offset => {
                let builder = new XmlPortBuilder();
                builder.showXmlPortWizard(offset);
        }));
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'alOutline.createQuery', 
            offset => {
                let builder = new QueryBuilder();
                builder.showQueryWizard(offset);
        }));
        context.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runPage', offset => {
                    ALObjectRunner.runInWebClient(offset);
                }));
        context.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runTable', 
                offset => {
                    ALObjectRunner.runInWebClient(offset);
                }));
        context.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runReport', 
                offset => {
                    ALObjectRunner.runInWebClient(offset);
                }));

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
