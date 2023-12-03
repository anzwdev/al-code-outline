import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddDotToToolTipModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Missing Dot to ToolTips", "addTooTipsEndingDots");
    }

}