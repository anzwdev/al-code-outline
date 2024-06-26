import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddReferencedTablesPermissionsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "addReferencedTablesPermissions", "addReferencedTablesPermissions");
    }

    protected getParameters(uri: vscode.Uri): any {
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        let parameters = super.getParameters(uri);
        parameters.sortProperties = this.sortPropertiesOnSave(uri);
        parameters.tablePermissionsOnly = false;
        return parameters;
    }

}