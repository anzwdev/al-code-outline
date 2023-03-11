import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class FormatDocumentModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Format Document", "formatDocument");
    }

}