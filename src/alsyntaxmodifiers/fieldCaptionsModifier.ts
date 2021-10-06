import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class FieldCaptionsModifier extends SyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "addFieldCaptions");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);

        let lockRemovedFields = !!vscode.workspace.getConfiguration('alOutline').get<boolean>('lockRemovedFieldsCaptions');
        parameters.lockRemovedFields = lockRemovedFields;

        return parameters;
    }

}