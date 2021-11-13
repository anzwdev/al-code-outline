import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class AddMissingParenthesesModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "addParentheses");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}