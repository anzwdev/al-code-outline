import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class FieldCaptionsModifier extends SyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "addFieldCaptions");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}