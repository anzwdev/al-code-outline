import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetDependenciesListRequest } from '../langserver/symbolsinformation/toolsGetDependenciesListRequest';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';
import { NumberHelper } from '../tools/numberHelper';
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RefreshToolTipsModifier extends WorkspaceCommandSyntaxModifier {
    private _dependencies: string[] | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Refresh ToolTips from Dependencies", "refreshToolTips");
        this._dependencies = undefined;
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        if (this._dependencies)
            for (let i=0; i<this._dependencies.length; i++) {
                let name = "dependencyName" + i.toString();
                parameters[name] = this._dependencies[i];
            }
        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to refresh tooltips from dependencies in all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected getSuccessWorkspaceMessage(response: ToolsWorkspaceCommandResponse): string {
        return NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +                    
            ' tooltip(s) updated in ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).';
    }

    protected getSuccessDocumentMessage(response: ToolsWorkspaceCommandResponse): string {
        return response.parameters.noOfChanges.toString() + 
            ' tooltip(s) updated.';
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        let defaultParameters = vscode.workspace.getConfiguration('alOutline', uri).get<string[]>('reuseToolTipsFromDependencies');
        if ((!defaultParameters) || (defaultParameters.length == 0))
            return false;
        this._dependencies = defaultParameters;
        return true;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        //load list of dependencies
        if (!uri)
            return false;
        let response = await this._context.toolsLangServerClient.getDependenciesList(new ToolsGetDependenciesListRequest(uri?.fsPath));
        if ((!response) || (!response.dependencies) || (response.dependencies.length == 0))
            return false;

        //ask for Application Area Type
        this._dependencies = await vscode.window.showQuickPick(response.dependencies, {
            canPickMany: true,
            placeHolder: 'Select Dependencies'
        });    

        return ((!!this._dependencies) && (this._dependencies.length > 0));
    }


}