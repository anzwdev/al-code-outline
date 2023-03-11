import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class AddTableDataCaptionFieldsModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Table Data Caption Fields", "addTableDataCaptionFields");
    }

    protected getParameters(uri: vscode.Uri): any {
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        let parameters = super.getParameters(uri);
        parameters.sortProperties = this.sortPropertiesOnSave(uri);
        this.addStringArrayToParameters(parameters, 'fieldsNamesPatterns', config.get<string[]>('tableDataCaptionFieldsNamesPatterns'));
        return parameters;
    }

}