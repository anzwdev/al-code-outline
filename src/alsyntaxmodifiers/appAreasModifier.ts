import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextEditorHelper } from '../tools/textEditorHelper';
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { SyntaxModifier } from './syntaxModifier';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';

export class AppAreasModifier extends SyntaxModifier {
    protected _appArea : string | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "addAppAreas");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
        this._appArea = undefined;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.appArea = this._appArea;
        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing application areas to all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected showWorkspaceSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +                    
            ' application area(s) added to ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).');
    }

    protected showDocumentSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            response.parameters.noOfChanges.toString() + 
            ' application area(s) added.');
    }

    protected async askForParameters() {
        let appAreasList = ['Basic', 'FixedAsset', 'All', 'Custom'];
        
        //ask for Application Area Type
        let appAreaName = await vscode.window.showQuickPick(appAreasList, {
            canPickMany: false,
            placeHolder: 'Select Application Area'
        });
        if (appAreaName === 'Custom')
            appAreaName = await vscode.window.showInputBox({
                placeHolder: "Enter your custom Application Area"
            });
        this._appArea = appAreaName;
    }

}
