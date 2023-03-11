import * as vscode from 'vscode';
import { AppPackageDocument } from './appPackageDocument';
import { ALSymbolsBrowser } from '../alsymbolsbrowser/alSymbolsBrowser';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALAppSymbolsLibrary } from '../symbollibraries/alAppSymbolsLibrary';

export class AppPackageEditorProvider implements vscode.CustomReadonlyEditorProvider {
    protected _devToolsContext: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        this._devToolsContext = devToolsContext;
    }

    openCustomDocument(uri: vscode.Uri, openContext: vscode.CustomDocumentOpenContext, token: vscode.CancellationToken): vscode.CustomDocument | Thenable<vscode.CustomDocument> {
        return new AppPackageDocument(uri);
    }

    resolveCustomEditor(document: vscode.CustomDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): void | Thenable<void> {
        let library = new ALAppSymbolsLibrary(this._devToolsContext, document.uri.fsPath);
        let symbolsBrowser = new ALSymbolsBrowser(this._devToolsContext, library);
        symbolsBrowser.attachToWebView(webviewPanel);
    }
     
}