import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class FixIdentifiersCaseModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Fix Identifiers Case", "fixIdentifiersCase");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        parameters.removeQuotesFromDataTypeIdentifiers = !!config.get<boolean>('fixCaseRemovesQuotesFromDataTypeIdentifiers');
        parameters.removeQuotesFromNonDataTypeIdentifiers = !!config.get<boolean>('fixCaseRemovesQuotesFromNonDataTypeIdentifiers');
        parameters.useBCLinterCopCaseRules = !!config.get<boolean>('useBCLinterCopCaseRules');
        return parameters;
    }

}