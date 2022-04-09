import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class SortVariablesModifier  extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Sort Variables", "sortVariables");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        parameters.sortMode = config.get<string>('variablesSortMode');
        return parameters;
    }

}