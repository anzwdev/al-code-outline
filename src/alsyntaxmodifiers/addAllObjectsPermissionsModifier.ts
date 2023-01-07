import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddAllObjectsPermissionsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "addAllObjectsPermissions", "addAllObjectsPermissions");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.excludeIncludedPermissionSetsPermissions = true;
        parameters.excludeExcludedPermissionSetsPermissions = true;
        return parameters;
    }

}