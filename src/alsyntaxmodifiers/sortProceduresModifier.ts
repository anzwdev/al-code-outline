import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortProceduresModifier  extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortProcedures");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}