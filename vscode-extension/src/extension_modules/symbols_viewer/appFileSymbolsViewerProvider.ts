import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AppFileSymbolsViewerDocument } from './appFileSymbolsViewerDocument';

export class AppFileSymbolsViewerProvider implements vscode.CustomReadonlyEditorProvider {

    protected context: DevToolsExtensionContext;

    constructor(devToolsContext: DevToolsExtensionContext) {
        this.context = devToolsContext;
    }

    openCustomDocument(uri: vscode.Uri, openContext: vscode.CustomDocumentOpenContext, token: vscode.CancellationToken): vscode.CustomDocument | Thenable<vscode.CustomDocument> {
        return new AppFileSymbolsViewerDocument(uri);
    }

    resolveCustomEditor(document: vscode.CustomDocument, webviewPanel: vscode.WebviewPanel, token: vscode.CancellationToken): void | Thenable<void> {
        this.context.symbolsViewerService.attachAppFileSymbolsViewer(document.uri.fsPath, webviewPanel);
    }
    
}