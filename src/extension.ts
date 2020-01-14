'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { DevToolsExtensionContext } from './devToolsExtensionContext';
import { ALAppSymbolsLibrary } from './symbollibraries/alAppSymbolsLibrary';
import { ALActionImageBrowser } from './actionimagebrowser/alActionImageBrowser';
import { ALNativeAppSymbolsLibrariesCache } from './symbollibraries/nativeimpl/alNativeAppSymbolsLibrariesCache';
import { AZSymbolsLibrary } from './symbollibraries/azSymbolsLibrary';
import { ALProjectSymbolsLibrary } from './symbollibraries/alProjectSymbolsLibrary';
import { ALCodeActionsProvider } from './codeactions/alCodeActionsProvider';
import { ALCodeAnalyzerFix } from './canalysisfix/AlCodeAnalyzerFix';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    const toolsExtensionContext: DevToolsExtensionContext = new DevToolsExtensionContext(context);
    let nativeAppCache: ALNativeAppSymbolsLibrariesCache | undefined = undefined;

    toolsExtensionContext.activeDocumentSymbols.loadAsync(false);
    context.subscriptions.push(toolsExtensionContext);

    //al app viewer
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.viewALApp',
            (fileUri) => {
                let uri: vscode.Uri = fileUri;
                let lib: AZSymbolsLibrary;

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

                toolsExtensionContext.showSymbolsBrowser(lib);
            }));

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.showAllProjectSymbols',
            () => {
                let workspacePath = toolsExtensionContext.alLangProxy.getCurrentWorkspaceFolderPath();
                if (workspacePath) {
                    let lib: ALProjectSymbolsLibrary = new ALProjectSymbolsLibrary(toolsExtensionContext,
                        true, workspacePath);
                    toolsExtensionContext.showSymbolsBrowser(lib);
                }
            }
        )
    );

    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.showProjectSymbolsWithoutDep',
            () => {
                let workspacePath = toolsExtensionContext.alLangProxy.getCurrentWorkspaceFolderPath();
                if (workspacePath) {
                    let lib: ALProjectSymbolsLibrary = new ALProjectSymbolsLibrary(toolsExtensionContext,
                        false, workspacePath);
                    toolsExtensionContext.showSymbolsBrowser(lib);
                }
            }
        )
    );

    //al action images viewer
    context.subscriptions.push(
        vscode.commands.registerCommand(
            'azALDevTools.viewActionImages',
            () => {
                let actionImageBrowser: ALActionImageBrowser = new ALActionImageBrowser(toolsExtensionContext);
                actionImageBrowser.show();
            }
        )
    );

    //code actions
    let alCodeActionsProvider: ALCodeActionsProvider = new ALCodeActionsProvider(toolsExtensionContext);
    context.subscriptions.push(
        vscode.languages.registerCodeActionsProvider('al', alCodeActionsProvider));

    // CodeAnalysis Fixes 

    let ALCodeAnalyzerFixer: ALCodeAnalyzerFix = new ALCodeAnalyzerFix(context);
    ALCodeAnalyzerFixer.loadActions();

    return toolsExtensionContext;
}

// this method is called when your extension is deactivated
export function deactivate() {
}
