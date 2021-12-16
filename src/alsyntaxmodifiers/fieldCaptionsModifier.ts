import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class FieldCaptionsModifier extends WorkspaceCommandSyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Table Field Captions", "addFieldCaptions");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);

        let lockRemovedFields = !!vscode.workspace.getConfiguration('alOutline').get<boolean>('lockRemovedFieldsCaptions');
        parameters.lockRemovedFields = lockRemovedFields;

        return parameters;
    }

}