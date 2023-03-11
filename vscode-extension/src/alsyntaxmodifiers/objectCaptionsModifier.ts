import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class ObjectCaptionsModifier extends WorkspaceCommandSyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Object Captions", "addObjectCaptions");
    }

}