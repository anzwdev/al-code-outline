import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortPropertiesModifier  extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortProperties");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}