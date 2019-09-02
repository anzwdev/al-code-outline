import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALDocCommentsProvider } from "../editorextensions/alDocCommentsProvider";

export class ALCompletionService {
    protected _context: DevToolsExtensionContext;
    protected _alDocCommentsProvider: ALDocCommentsProvider;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;

        //documentation completion provider
        this._alDocCommentsProvider= new ALDocCommentsProvider(this._context);
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.languages.registerCompletionItemProvider('al', this._alDocCommentsProvider, '/'));
    }
}