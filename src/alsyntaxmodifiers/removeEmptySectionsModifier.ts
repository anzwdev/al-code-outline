import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NameValueQuickPickItem } from '../tools/nameValueQuickPickItem';
import { IRemoveEmptySectionsSettings } from "./iRemoveEmptySectionsSettings";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveEmptySectionsModifier extends WorkspaceCommandSyntaxModifier {
    protected _settings: IRemoveEmptySectionsSettings;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove Empty Sections", "removeEmptySections");
        this._settings = {
            removePageFieldGroups: true,
            removeActionGroups: true,
            removeActions: true,
            ignoreComments: false
        };
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        this.copySettings(parameters, this._settings);
        return parameters;
    }

    protected copySettings(dest: any, src: any) {
        if (!src)
            src = {};
        dest.removePageFieldGroups = !!src.removePageFieldGroups;
        dest.removeActionGroups = !!src.removeActionGroups;
        dest.removeActions = !!src.removeActions;
        dest.ignoreComments = !!src.ignoreComments;
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        let defaultParameters = vscode.workspace.getConfiguration('alOutline', uri).get<IRemoveEmptySectionsSettings>('defaultRemoveEmptySectionsSettings');
        if (!defaultParameters)
            return false;
        this.copySettings(this._settings, defaultParameters);
        return true;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        this.loadState();
        let quickPickItems = [
            new NameValueQuickPickItem('Empty Page Field Groups', 'removePageFieldGroups', !!this._settings.removePageFieldGroups),
            new NameValueQuickPickItem('Empty Action Groups', 'removeActionGroups', !!this._settings.removeActionGroups),
            new NameValueQuickPickItem('Empty Actions', 'removeActions', !!this._settings.removeActions),
            new NameValueQuickPickItem('Ignore comments in method body', 'ignoreComments', !!this._settings.ignoreComments)
        ];

        let selectedValues = await vscode.window.showQuickPick(
            quickPickItems, { canPickMany: true, placeHolder: 'Select elements to remove' });

        if (!selectedValues)
            return false;

        this.clearSettings();
        let data: any = {};
        if (selectedValues) {
            for (let i=0; i<selectedValues.length; i++) {
                data[selectedValues[i].value] = true;
            }
        }
            
        this.copySettings(this._settings, data);
        this.saveState();

        return true;
    }

    private loadState() {
        let vsctx = this._context.vscodeExtensionContext;
        this._settings.removePageFieldGroups = this.getBoolSetting(vsctx, "azALDevTools.remESections.removePageFieldGroups", !!this._settings.removePageFieldGroups);
        this._settings.removeActionGroups = this.getBoolSetting(vsctx, "azALDevTools.remESectionsremoveActionGroups", !!this._settings.removeActionGroups);
        this._settings.removeActions = this.getBoolSetting(vsctx, "azALDevTools.remESectionsremoveActions", !!this._settings.removeActions);
        this._settings.ignoreComments = this.getBoolSetting(vsctx, "azALDevTools.remESectionsignoreComments", !!this._settings.ignoreComments);
    }

    private getBoolSetting(vsctx: vscode.ExtensionContext, name: string, defVal: boolean): boolean {
        let val = vsctx.globalState.get<boolean>(name);
        if (val === undefined)
            return defVal;
        return !!val;
    }

    private saveState() {
        let vsctx = this._context.vscodeExtensionContext;
        vsctx.globalState.update("azALDevTools.remESections.removePageFieldGroups", !!this._settings.removePageFieldGroups);
        vsctx.globalState.update("azALDevTools.remESectionsremoveActionGroups", !!this._settings.removeActionGroups);
        vsctx.globalState.update("azALDevTools.remESectionsremoveActions", !!this._settings.removeActions);
        vsctx.globalState.update("azALDevTools.remESectionsignoreComments", !!this._settings.ignoreComments);
    }

    private clearSettings() {
        this._settings.removePageFieldGroups = false;
        this._settings.removeActionGroups = false;
        this._settings.removeActions = false;
        this._settings.ignoreComments = false;
    }

}