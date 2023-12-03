import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddUsingRegionModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Using Region", "addUsingRegion");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        let settings = vscode.workspace.getConfiguration('alOutline', uri);                
        parameters.title = settings.get<string>('usingsRegionTitle');
        return parameters;
    }

}