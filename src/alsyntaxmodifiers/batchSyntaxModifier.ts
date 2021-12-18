import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextRange } from '../symbollibraries/textRange';
import { ISyntaxModifierResult } from './iSntaxModifierResult';
import { SyntaxModifier } from "./syntaxModifier";

export class BatchSyntaxModifier extends SyntaxModifier {
    protected _modifiers: SyntaxModifier[] | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, 'Code Cleanup');
        this._modifiers = undefined;
        this._progressMessage = '';
    }

    async runForWorkspaceWithoutUI(workspaceUri: vscode.Uri): Promise<ISyntaxModifierResult | undefined> {
        if (this._showProgress)
            return await vscode.window.withProgress<ISyntaxModifierResult | undefined>({
                    location: vscode.ProgressLocation.Notification,
                    title: this._progressMessage
                }, async (progress) => {
                    return await this.runForWorkspaceWithoutUIWithProgress(workspaceUri, progress);
                });
        return await this.runForWorkspaceWithoutUIWithProgress(workspaceUri, undefined);
    }    

    async runForWorkspaceWithoutUIWithProgress(workspaceUri: vscode.Uri, progress: vscode.Progress<{ message?: string; increment?: number }> | undefined): Promise<ISyntaxModifierResult | undefined> {
        let allMessages = '';

        if (this._modifiers) {
            let count = this._modifiers.length;
            for (let i=0; i<count; i++) {
                if (progress)
                    progress.report({
                        message: 'Runnung Command ' + (i+1).toString() + ' of ' + count.toString() + ': ' + this._modifiers[i].name, 
                        increment: (100 * i / count)});
                        
                let result = await this._modifiers[i].runForWorkspaceWithoutUI(workspaceUri);
                allMessages = this.appendResult(allMessages, this._modifiers[i].name, result);
            }
        }

        return {
            success: true,
            message: allMessages,
            source: undefined
        };
    }    

    async runForDocumentWithoutUI(text: string, workspaceUri: vscode.Uri, documentUri: vscode.Uri, range: TextRange | undefined): Promise<ISyntaxModifierResult | undefined> {
        if (this._showProgress)
            return await vscode.window.withProgress<ISyntaxModifierResult | undefined>({
                    location: vscode.ProgressLocation.Notification,
                    title: this._progressMessage
                }, async (progress) => {
                    return await this.runForDocumentWithoutUIWithProgress(text, workspaceUri, documentUri, range, progress);
                });
        return await this.runForDocumentWithoutUIWithProgress(text, workspaceUri, documentUri, range, undefined);
    }

    protected async runForDocumentWithoutUIWithProgress(text: string, workspaceUri: vscode.Uri, documentUri: vscode.Uri, range: TextRange | undefined, progress: vscode.Progress<{ message?: string; increment?: number }> | undefined): Promise<ISyntaxModifierResult | undefined> {
        let allMessages = '';

        if (this._modifiers) {
            let count = this._modifiers.length;
            for (let i=0; i<count; i++) {                
                if (progress)
                    progress.report({
                        message: 'Runnung Command ' + (i+1).toString() + ' of ' + count.toString() + ': ' + this._modifiers[i].name, 
                        increment: (100 * i / count)});

                let result = await this._modifiers[i].runForDocumentWithoutUI(text, workspaceUri, documentUri, range);
                if ((result) && (result.success) && (result.source) && (result.source != ''))
                    text = result.source;
                allMessages = this.appendResult(allMessages, this._modifiers[i].name, result);
            }
        }

        return {
            success: true,
            message: allMessages,
            source: text
        };
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        if (!this.collectModifiers(uri))
            return false;

        if ((this._modifiers) && (this._modifiers.length > 0))  { 
            for (let i=0; i<this._modifiers.length; i++) {
                let cont = await this._modifiers[i].askForParameters(uri);
                if (!cont)
                    return false;
            }
            return true;
        }

        vscode.window.showErrorMessage('Please specify list of commands that you would like to run in the "alOutline.codeCleanupActions" setting.');
        return false;
    }

    protected appendResult(allMessages: string, name: string, result: ISyntaxModifierResult | undefined): string {
        if (result) {
            allMessages = allMessages + name + ": ";
            if (result.success)
                allMessages = allMessages + "success"
            else
                allMessages = allMessages + "error";
            if (result.message)
                allMessages = allMessages + " - " + result.message;
            allMessages = allMessages + ", ";
        }
        return allMessages;
    }

    protected collectModifiers(uri: vscode.Uri | undefined): boolean {
        let modifiersList: SyntaxModifier[] = [];

        //get modifiers names
        let actionNames = vscode.workspace.getConfiguration('alOutline', uri).get<string[]>('codeCleanupActions');
        if ((actionNames) && (actionNames.length > 0)) {
            for (let i=0; i<actionNames.length; i++) {
                let modifier = this._context.alCodeTransformationService.getSyntaxModifier(actionNames[i]);
                if (modifier) {
                    modifier.hideProgress();
                    modifiersList.push(modifier);
                } else {
                    this._modifiers = [];
                    vscode.window.showErrorMessage('Uknnown command in "alOutline.codeCleanupActions" setting: ' + actionNames[i]);
                    return false;
                }
            }
        }

        this._modifiers = modifiersList;
        return true;
    }

}