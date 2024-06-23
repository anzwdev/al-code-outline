import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class SortProceduresOrTriggersModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext, name: string, commandName: string) {
        super(context, name, commandName);
    }

    protected getParameters(uri: vscode.Uri): any {      
        let parameters = super.getParameters(uri);
        let settings = vscode.workspace.getConfiguration('alOutline', uri);                
        parameters.triggersSortMode = settings.get<string>('triggersSortMode');
        parameters.triggersNaturalOrder = JSON.stringify(settings.get('triggersNaturalOrder'));
        parameters.sortSingleNodeRegions = !!settings.get<boolean>('sortSingleNodeRegions');
        parameters.globalVariablesAfterTriggers = !!settings.get<boolean>('sortMembersPutGlobalVariablesAfterTriggers');
        return parameters;
    }

}