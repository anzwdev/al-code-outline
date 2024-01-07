import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveUnusedUsingsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove unused usings", "updateUsingsList");
    }

    protected getParameters(uri: vscode.Uri): any {      
        let parameters = super.getParameters(uri);
        parameters.removeUnusedUsings = true;
        parameters.addMissingUsings = false;
        return parameters;
    }

}