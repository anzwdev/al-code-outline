import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddDropDownFieldGroupsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add DropDown Field Groups", "addDropDownFieldGroups");
    }

    protected getParameters(uri: vscode.Uri): any {
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        let parameters = super.getParameters(uri);
        this.addStringArrayToParameters(parameters, 'fieldsNamesPatterns', config.get<string[]>('dropDownGroupFieldsNamesPatterns'));
        return parameters;
    }

}