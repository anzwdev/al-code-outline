'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as vzFileTemplates from 'vz-file-templates';
import { ALObjectRunner } from './alObjectRunner';
import { DevToolsExtensionContext } from './devToolsExtensionContext';
import { SymbolsTreeProvider } from './outlineview/symbolsTreeProvider';
import { ALAppSymbolsLibrary } from './symbollibraries/alAppSymbolsLibrary';
import { ALSymbolsBrowser } from './alsymbolsbrowser/alSymbolsBrowser';
import { ALActionImageBrowser } from './actionimagebrowser/alActionImageBrowser';
import { ALAppFileViewer } from './alappviewer/alAppFileViewer';
import { PageBuilder } from './objectbuilders/pageBuilder';
import { ReportBuilder } from './objectbuilders/reportBuilder';
import { XmlPortBuilder } from './objectbuilders/xmlPortBuilder';
import { QueryBuilder } from './objectbuilders/queryBuilder';
import { ALNativeAppSymbolsLibrariesCache } from './symbollibraries/nativeimpl/alNativeAppSymbolsLibrariesCache';
import { AZSymbolsLibrary } from './symbollibraries/azSymbolsLibrary';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    const toolsExtensionContext : DevToolsExtensionContext = new DevToolsExtensionContext(context);
    const symbolsTreeProvider : SymbolsTreeProvider = new SymbolsTreeProvider(toolsExtensionContext);
    let nativeAppCache: ALNativeAppSymbolsLibrariesCache | undefined = undefined;

    toolsExtensionContext.activeDocumentSymbols.loadAsync(false);

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
                let lib : AZSymbolsLibrary;

                

                if (toolsExtensionContext.toolsLangServerClient.isEnabled())
                    lib = new ALAppSymbolsLibrary(toolsExtensionContext, uri.fsPath);
                else {
                    //if language server is not available on this platform or with currently active
                    //version of Microsoft AL Extension, then we have to use simplified native implementation
                    //of app package reader
                    if (!nativeAppCache)
                        nativeAppCache = new ALNativeAppSymbolsLibrariesCache(toolsExtensionContext);
                    lib = nativeAppCache.getOrCreate(uri.fsPath);
                }

                //let useNewViewer = vscode.workspace.getConfiguration('alOutline').get('enableFeaturePreview');
                if (toolsExtensionContext.getUseSymbolsBrowser()) {
                    let objectBrowser : ALSymbolsBrowser = new ALSymbolsBrowser(toolsExtensionContext, lib);
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

    return toolsExtensionContext;
}

// this method is called when your extension is deactivated
export function deactivate() {
}
