import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddMissingUsingsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add missing usings", "updateUsingsList");
    }

    protected getParameters(uri: vscode.Uri): any {      
        let parameters = super.getParameters(uri);
        parameters.removeUnusedUsings = false;
        parameters.addMissingUsings = true;
        return parameters;
    }

}