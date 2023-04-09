import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveRedundantDataClassificationModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove Redundant DataClassification", "removeRedundantDataClassification");
    }

}