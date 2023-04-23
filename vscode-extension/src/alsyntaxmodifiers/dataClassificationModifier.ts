import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class DataClassificationModifier extends WorkspaceCommandSyntaxModifier {
    protected _dataClassification : string | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Data Classification", "addDataClassification");
        this._dataClassification = undefined;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.dataClassification = this._dataClassification;
        parameters.sortProperties = this.sortPropertiesOnSave(uri);
        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing data classification and remove them from flowfields in all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected getSuccessWorkspaceMessage(response: ToolsWorkspaceCommandResponse): string {
        return NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +                    
            ' data classification(s) added/removed to/from ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).';
    }

    protected getSuccessDocumentMessage(response: ToolsWorkspaceCommandResponse): string {
        return response.parameters.noOfChanges.toString() + 
            ' data classification(s) added or removed.';
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        this._dataClassification = vscode.workspace.getConfiguration('alOutline', uri).get<string>('defaultDataClassification');
        return ((!!this._dataClassification) && (this._dataClassification != ''));
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        let valuesList = ['AccountData', 'CustomerContent',
            'EndUserIdentifiableInformation', 'EndUserPseudonymousIdentifiers',
            'OrganizationIdentifiableInformation', 'SystemMetadata'];

        //ask for Application Area Type
        this._dataClassification = await vscode.window.showQuickPick(valuesList, {
            canPickMany: false,
            placeHolder: 'Select Data Classification'
        });    

        return !!this._dataClassification;
    }

}