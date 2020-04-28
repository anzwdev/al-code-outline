import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALCodeActionsProvider } from '../codeactions/alCodeActionsProvider';

export class ALCodeActionsService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;

        //code actions
        let alCodeActionsProvider: ALCodeActionsProvider = new ALCodeActionsProvider(this._context);
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.languages.registerCodeActionsProvider('al', alCodeActionsProvider));

    }

}