import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortVariablesModifier  extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortVariables");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}