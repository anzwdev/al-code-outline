import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortTableFieldsModifier  extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortTableFields");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}