import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALCodeActionsProvider } from '../codeactions/alCodeActionsProvider';
import { DevToolsExtensionService } from './devToolsExtensionService';

export class ALCodeActionsService extends DevToolsExtensionService {

    constructor(context: DevToolsExtensionContext) {
        super(context);

        //code actions
        let alCodeActionsProvider: ALCodeActionsProvider = new ALCodeActionsProvider(this._context);
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.languages.registerCodeActionsProvider('al', alCodeActionsProvider));

    }

}