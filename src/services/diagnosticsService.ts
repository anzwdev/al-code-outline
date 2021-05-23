import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextEditorHelper } from '../tools/textEditorHelper';

export class DiagnosticsService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {        
        this._context = context;
        this.registerCommands();
    }

    protected registerCommands() {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showExtensionLog',
                (fileUri) => {
                    this.showErrorLog();
                }
            )
        );
    }

    protected showErrorLog() {
        if (this._context.toolsLangServerClient.errorLogUri)
            TextEditorHelper.openEditor(this._context.toolsLangServerClient.errorLogUri, true, true, undefined);
        else
            vscode.window.showErrorMessage('Extension log file is not available');
    }

}