import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NameValueQuickPickItem } from '../tools/nameValueQuickPickItem';
import { IRemoveUnusedVariablesSettings } from './iRemoveUnusedVariablesSettings';
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class RemoveUnusedVariablesModifier extends WorkspaceCommandSyntaxModifier {
    protected _variableTypes: IRemoveUnusedVariablesSettings;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove Unused Variables", "removeUnusedVariables");
        this._variableTypes = {
            removeGlobalVariables: false,
            removeLocalVariables: false,
            removeLocalMethodParameters: false
        };
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        this.copySettings(parameters, this._variableTypes);
        return parameters;
    }

    protected areSettingsEmpty(value: IRemoveUnusedVariablesSettings | undefined) {
        return ((!value) || (
            (!value.removeGlobalVariables) &&
            (!value.removeLocalVariables) &&
            (!value.removeLocalMethodParameters)));
    }

    protected copySettings(dest: any, src: any) {
        if (!src)
            src = {};
        dest.removeGlobalVariables = !!src.removeGlobalVariables;
        dest.removeLocalVariables = !!src.removeLocalVariables;
        dest.removeLocalMethodParameters = !!src.removeLocalMethodParameters;
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        let defaultParameters = vscode.workspace.getConfiguration('alOutline', uri).get<IRemoveUnusedVariablesSettings>('defaultRemoveUnusedVariablesSettings');
        if (this.areSettingsEmpty(defaultParameters))
            return false;
        this.copySettings(this._variableTypes, defaultParameters);
        return true;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        this.loadState();
        let quickPickItems = [
            new NameValueQuickPickItem('Global variables', 'removeGlobalVariables', !!this._variableTypes.removeGlobalVariables),
            new NameValueQuickPickItem('Local variables', 'removeLocalVariables', !!this._variableTypes.removeLocalVariables),
            new NameValueQuickPickItem('Local methods parameters', 'removeLocalMethodParameters', !!this._variableTypes.removeLocalMethodParameters)
        ];

        let selectedValues = await vscode.window.showQuickPick(
            quickPickItems, { canPickMany: true, placeHolder: 'Select variables to remove' });

        if (!selectedValues)
            return false;

        this.clearVariableTypes();
        let data: any = {};
        if (selectedValues) {
            for (let i=0; i<selectedValues.length; i++) {
                data[selectedValues[i].value] = true;
            }
        }
        if (this.areSettingsEmpty(data))
            return false;
            
        this.copySettings(this._variableTypes, data);
        this.saveState();

        return true;
    }

    private loadState() {
        let vsctx = this._context.vscodeExtensionContext;
        this._variableTypes.removeGlobalVariables = !!vsctx.globalState.get<boolean>("azALDevTools.remUVar.removeGlobalVariables");
        this._variableTypes.removeLocalVariables = !!vsctx.globalState.get<boolean>("azALDevTools.remUVar.removeLocalVariables");
        this._variableTypes.removeLocalMethodParameters = !!vsctx.globalState.get<boolean>("azALDevTools.remUVar.removeLocalMethodParameters");
        //set defaults
        if ((!this._variableTypes.removeGlobalVariables) &&
            (!this._variableTypes.removeLocalVariables) &&
            (!this._variableTypes.removeLocalMethodParameters)) {

            this._variableTypes.removeGlobalVariables = true;
            this._variableTypes.removeLocalVariables = true;
            this._variableTypes.removeLocalMethodParameters = false;
        }
    }

    private saveState() {
        let vsctx = this._context.vscodeExtensionContext;
        vsctx.globalState.update("azALDevTools.remUVar.removeGlobalVariables", !!this._variableTypes.removeGlobalVariables);
        vsctx.globalState.update("azALDevTools.remUVar.removeLocalVariables", !!this._variableTypes.removeLocalVariables);
        vsctx.globalState.update("azALDevTools.remUVar.removeLocalMethodParameters", !!this._variableTypes.removeLocalMethodParameters);
    }

    private clearVariableTypes() {
        this._variableTypes.removeGlobalVariables = false;
        this._variableTypes.removeLocalVariables = false;
        this._variableTypes.removeLocalMethodParameters = false;
    }


}