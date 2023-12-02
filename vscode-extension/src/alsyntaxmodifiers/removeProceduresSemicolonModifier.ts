import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveProceduresSemicolonModifier extends WorkspaceCommandSyntaxModifier {
    
    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove procedure semicolon", "removeProceduresSemicolon");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        let settings = vscode.workspace.getConfiguration('alOutline', uri);                
        parameters.includeInterfaces = !!settings.get<boolean>('removeProceduresSemicolonIncludeInterfaces');
        return parameters;
    }

}
