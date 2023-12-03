import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class SortTableFieldsModifier  extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Sort Table Fields", "sortTableFields");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        parameters.sortSingleNodeRegions = !!config.get<boolean>('sortSingleNodeRegions');

        return parameters;
    }

}