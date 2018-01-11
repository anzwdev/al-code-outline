'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import { ALOutlineProvider } from './aloutline';

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {
    const alOutlineProvider = new ALOutlineProvider(context);

    vscode.window.registerTreeDataProvider('alOutline', alOutlineProvider);

    vscode.commands.registerCommand('alOutline.refresh', () => alOutlineProvider.refresh());
    vscode.commands.registerCommand('alOutline.createCardPage', offset => alOutlineProvider.createCardPage(offset));
    vscode.commands.registerCommand('alOutline.createListPage', offset => alOutlineProvider.createListPage(offset));
	vscode.commands.registerCommand('alOutline.openSelection', range => {
		alOutlineProvider.select(range);
	});
}

// this method is called when your extension is deactivated
export function deactivate() {
}
