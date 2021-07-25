import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class FixKeywordsCaseModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "fixKeywordsCase");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }


}