import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class ObjectCaptionsModifier extends SyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "addObjectCaptions");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}