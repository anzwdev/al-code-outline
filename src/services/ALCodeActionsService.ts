import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALCAFixesCodeActionsProvider } from '../canalyzerfixes/alCAFixesCodeActionsProvider';
import { ALCodeActionsProvider } from '../codeactions/alCodeActionsProvider';

export class ALCodeActionsService {
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;

        //code analyzers fixes actions
        let alCAFixesCodeActionsProvider: ALCAFixesCodeActionsProvider = new ALCAFixesCodeActionsProvider();
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.languages.registerCodeActionsProvider('al', alCAFixesCodeActionsProvider));

        //code actions
        let alCodeActionsProvider: ALCodeActionsProvider = new ALCodeActionsProvider(this._context);
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.languages.registerCodeActionsProvider('al', alCodeActionsProvider));

    }

}