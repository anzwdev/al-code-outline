import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortPermissionsModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortPermissions");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}