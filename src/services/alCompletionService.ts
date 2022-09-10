import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALCompletionProvider } from '../editorextensions/alCompletionProvider';
import { ALDocCommentsProvider } from "../editorextensions/alDocCommentsProvider";
import { SyntaxFilesContentProvider } from '../fileproviders/syntaxFilesContentProvider';
import { DevToolsExtensionService } from './devToolsExtensionService';

export class ALCompletionService extends DevToolsExtensionService {
    protected _alDocCommentsProvider: ALDocCommentsProvider | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        //documentation completion provider
        if (this._context.alLangProxy.version.major < 6) {
            this._alDocCommentsProvider= new ALDocCommentsProvider(this._context);
            this._context.vscodeExtensionContext.subscriptions.push(
                vscode.languages.registerCompletionItemProvider('al', this._alDocCommentsProvider, '/'));
        }

        //language server based completion provider
        vscode.languages.registerCompletionItemProvider('al', new ALCompletionProvider(context));

        //json files completion implemented by providing additional file schemas
        let syntaxProvider = new SyntaxFilesContentProvider(this._context);
        this._context.vscodeExtensionContext.subscriptions.push(
                vscode.workspace.registerTextDocumentContentProvider(syntaxProvider.scheme, syntaxProvider));

    }
}