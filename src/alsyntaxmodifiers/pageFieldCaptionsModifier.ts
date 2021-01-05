import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class PageFieldCaptionsModifier extends SyntaxModifier {
    protected _useNameIfNoCaption: boolean;

    constructor(context: DevToolsExtensionContext) {
        super(context, "addPageFieldCaptions");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
        this._useNameIfNoCaption = false;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.useNameIfNoCaption = this._useNameIfNoCaption;
        return parameters;
    }

}