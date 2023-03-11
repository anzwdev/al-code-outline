import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveEmptyLinesModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove Empty Lines", "removeEmptyLines");
    }

}