import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class MakeFlowFieldsReadOnlyModifier extends WorkspaceCommandSyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "Make FlowFields Read-Only", "makeFlowFieldsReadOnly");
    }

}