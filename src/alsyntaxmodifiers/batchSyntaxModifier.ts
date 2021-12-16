import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextRange } from '../symbollibraries/textRange';
import { ISyntaxModifierResult } from './iSntaxModifierResult';
import { SyntaxModifier } from "./syntaxModifier";

export class BatchSyntaxModifier extends SyntaxModifier {
    protected _modifiers: SyntaxModifier[] | undefined;


    constructor(context: DevToolsExtensionContext) {
        super(context, "Code Cleanup");
        this._modifiers = undefined;
    }

    async runForWorkspaceWithoutUI(workspaceUri: vscode.Uri): Promise<ISyntaxModifierResult | undefined> {
        let allMessages = '';

        if (this._modifiers) {
            for (let i=0; i<this._modifiers.length; i++) {
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

    async RunForDocumentWithoutUI(text: string, workspaceUri: vscode.Uri, documentUri: vscode.Uri, range: TextRange | undefined): Promise<ISyntaxModifierResult | undefined> {
        let allMessages = '';

        if (this._modifiers) {
            for (let i=0; i<this._modifiers.length; i++) {
                let result = await this._modifiers[i].RunForDocumentWithoutUI(text, workspaceUri, documentUri, range);
                if ((result) && (result.source) && (result.source != ''))
                    text = result.source;
                allMessages = this.appendResult(allMessages, this._modifiers[i].name, result);
            }
        }

        return {
            success: true,
            message: allMessages,
            source: undefined
        };
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        this.collectModifiers(uri);

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
            allMessages = allMessages + "\n";
        }
        return allMessages;
    }

    protected collectModifiers(uri: vscode.Uri | undefined) {
        let modifiersList: SyntaxModifier[] = [];

        //get modifiers names
        let actionNames = vscode.workspace.getConfiguration('alOutline', uri).get<string[]>('codeActionsOnSave');
        if ((actionNames) && (actionNames.length > 0)) {
            for (let i=0; i<actionNames.length; i++) {
                let modifier = this._context.alCodeTransformationService.getSyntaxModifier(actionNames[i]);
                if (modifier)
                    modifiersList.push(modifier);
            }
        }

        this._modifiers = modifiersList;
    }

}