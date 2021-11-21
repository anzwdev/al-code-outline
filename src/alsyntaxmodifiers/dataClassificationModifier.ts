import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextEditorHelper } from '../tools/textEditorHelper';
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { SyntaxModifier } from './syntaxModifier';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';

export class DataClassificationModifier extends SyntaxModifier {
    protected _dataClassification : string | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "addDataClassification");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
        this._dataClassification = undefined;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.dataClassification = this._dataClassification;
        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing data classification to all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected showWorkspaceSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +                    
            ' data classification(s) added to ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).');
    }

    protected showDocumentSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            response.parameters.noOfChanges.toString() + 
            ' data classification(s) added.');
    }

    protected async askForParameters() {
        let valuesList = ['AccountData', 'CustomerContent',
            'EndUserIdentifiableInformation', 'EndUserPseudonymousIdentifiers',
            'OrganizationIdentifiableInformation', 'SystemMetadata'];

        //ask for Application Area Type
        this._dataClassification = await vscode.window.showQuickPick(valuesList, {
            canPickMany: false,
            placeHolder: 'Select Data Classification'
        });
    }

}