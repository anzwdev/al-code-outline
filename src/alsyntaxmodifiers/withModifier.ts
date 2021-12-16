import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class WithModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove With", "removeWith");
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        if (this._context.alLangProxy.version.major < 6) {
            vscode.window.showErrorMessage("To remove WITH statements with the current AL project you have to be using Microsoft AL Extension version 6 or newer.");
            return false;
        }
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to run this command for all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
    }


}