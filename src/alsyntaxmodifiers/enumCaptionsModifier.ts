import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class EnumCaptionsModifier  extends WorkspaceCommandSyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Enum Values Captions", "addEnumCaptions");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.sortProperties = this.sortPropertiesOnSave(uri);
        return parameters;
    }

}