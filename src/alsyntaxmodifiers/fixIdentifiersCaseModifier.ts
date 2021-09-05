import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class FixIdentifiersCaseModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "fixIdentifiersCase");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }


}