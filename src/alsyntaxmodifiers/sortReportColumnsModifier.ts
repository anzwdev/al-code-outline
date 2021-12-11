import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SyntaxModifier } from "./syntaxModifier";

export class SortReportColumnsModifier  extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "sortReportColumns");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

}