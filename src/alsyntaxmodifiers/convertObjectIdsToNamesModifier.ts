import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class ConvertObjectIdsToNamesModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "convertObjectIdsToNames");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}