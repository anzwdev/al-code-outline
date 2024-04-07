import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class ObjectCaptionsModifier extends WorkspaceCommandSyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Object Captions", "addObjectCaptions");
    }

    protected getParameters(uri: vscode.Uri): any {
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        let parameters = super.getParameters(uri);
        parameters.sortProperties = this.sortPropertiesOnSave(uri);
        return parameters;
    }

}