import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class AppAreasModifier extends WorkspaceCommandSyntaxModifier {
    protected _appArea : string | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add App Areas", "addAppAreas");
        this._appArea = undefined;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.appArea = this._appArea;
        parameters.appAreaMode = this._context.alLangProxy.getAppAreaMode(uri);
        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing application areas to all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected getSuccessWorkspaceMessage(response: ToolsWorkspaceCommandResponse): string {
        return NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +                    
            ' application area(s) added to ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).';
    }

    protected getSuccessDocumentMessage(response: ToolsWorkspaceCommandResponse): string {
        return response.parameters.noOfChanges.toString() + 
            ' application area(s) added.';
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        let settings = vscode.workspace.getConfiguration('alOutline', uri);                
        this._appArea = settings.get<string>('defaultAppArea');
        return ((!!this._appArea) && (this._appArea != ''));
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        let appAreasList = ['Basic', 'FixedAsset', 'All', 'Custom'];
        
        //ask for Application Area Type
        let appAreaName = await vscode.window.showQuickPick(appAreasList, {
            canPickMany: false,
            placeHolder: 'Select Application Area'
        });
        if (!appAreaName)
            return false;
        if (appAreaName === 'Custom') {
            appAreaName = await vscode.window.showInputBox({
                placeHolder: "Enter your custom Application Area"
            });
            if (!appAreaName)
                return false;
        }
        this._appArea = appAreaName;

        return true;
    }

}
