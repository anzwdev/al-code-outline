import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortPermissionSetListModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortPermissionSetList");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}