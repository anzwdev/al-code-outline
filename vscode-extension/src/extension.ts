// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { DevToolsExtensionContext } from './devToolsExtensionContext';

// This method is called when your extension is activated
// Your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    const toolsExtensionContext: DevToolsExtensionContext = new DevToolsExtensionContext(context);
    toolsExtensionContext.activeDocumentSymbols.initializeContent();
    context.subscriptions.push(toolsExtensionContext);
    vscode.commands.executeCommand('setContext', 'azALDevTools.extensionActive', true);
    return toolsExtensionContext;
}

// This method is called when your extension is deactivated
export function deactivate() {
    vscode.commands.executeCommand('setContext', 'azALDevTools.extensionActive', false);
}
