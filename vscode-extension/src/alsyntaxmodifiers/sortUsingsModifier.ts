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
        parameters.namespacesOrder = this.mergeList(settings.get<string[]>('namespacesSortOrder') || [], ';');
        return parameters;
    }

    protected mergeList(list: string[] | undefined, separator: string): string | undefined {
        if ((!list) || (list.length === 0)) {
            return undefined;
        }
        if (list.length === 1) {
            return list[0];
        }

        let result: string = list[0];
        for (let i=0; i<list.length; i++) {
            result += separator + list[i];
        }

        return result;
    }

}