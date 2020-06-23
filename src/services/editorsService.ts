import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AppJsonEditorProvider } from '../editors/appjsoneditor/appJsonEditorProvider';

export class EditorsService {
    protected _context: DevToolsExtensionContext;   

    constructor(newContext: DevToolsExtensionContext) {
        this._context = newContext;
        this.registerEditors();
    }

    protected registerEditors() {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerCustomEditorProvider('azALDevTools.appJsonEditor',
                new AppJsonEditorProvider(this._context)));
    }

}