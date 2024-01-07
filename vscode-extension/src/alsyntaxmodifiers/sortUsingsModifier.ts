import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class SortUsingsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Sort Usings", "sortUsings");
    }

    protected getParameters(uri: vscode.Uri): any {      
        let parameters = super.getParameters(uri);
        let settings = vscode.workspace.getConfiguration('alOutline', uri);                
        parameters.sortSingleNodeRegions = !!settings.get<boolean>('sortSingleNodeRegions');
        return parameters;
    }

}