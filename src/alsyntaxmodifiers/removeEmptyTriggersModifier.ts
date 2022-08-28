import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NameValueQuickPickItem } from '../tools/nameValueQuickPickItem';
import { IRemoveEmptyTriggersSettings } from "./iRemoveEmptyTriggersSettings";
import { WorkspaceCommandSyntaxModifier } from "./workspaceCommandSyntaxModifier";

export class RemoveEmptyTriggersModifier extends WorkspaceCommandSyntaxModifier {
    protected _settings: IRemoveEmptyTriggersSettings;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Remove Empty Triggers", "removeEmptyTriggers");
        this._settings = {
            removeTriggers: true,
            removeSubscribers: true
        };
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        this.copySettings(parameters, this._settings);
        return parameters;
    }

    protected areSettingsEmpty(value: IRemoveEmptyTriggersSettings | undefined) {
        return ((!value) || (
            (!value.removeTriggers) &&
            (!value.removeSubscribers)));
    }

    protected copySettings(dest: any, src: any) {
        if (!src)
            src = {};
        dest.removeTriggers = !!src.removeTriggers;
        dest.removeSubscribers = !!src.removeSubscribers;
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        let defaultParameters = vscode.workspace.getConfiguration('alOutline', uri).get<IRemoveEmptyTriggersSettings>('defaultRemoveEmptyTriggersSettings');
        if (this.areSettingsEmpty(defaultParameters))
            return false;
        this.copySettings(this._settings, defaultParameters);
        return true;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        this.loadState();
        let quickPickItems = [
            new NameValueQuickPickItem('Empty Triggers', 'removeTriggers', !!this._settings.removeTriggers),
            new NameValueQuickPickItem('Empty Event Subscribers', 'removeSubscribers', !!this._settings.removeSubscribers)
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
        if (this.areSettingsEmpty(data))
            return false;
            
        this.copySettings(this._settings, data);
        this.saveState();

        return true;
    }

    private loadState() {
        let vsctx = this._context.vscodeExtensionContext;
        this._settings.removeTriggers = !!vsctx.globalState.get<boolean>("azALDevTools.remETrig.removeTriggers");
        this._settings.removeSubscribers = !!vsctx.globalState.get<boolean>("azALDevTools.remETrig.removeSubscribers");
        //set defaults
        if ((!this._settings.removeTriggers) &&
            (!this._settings.removeSubscribers)) {

            this._settings.removeTriggers = true;
            this._settings.removeSubscribers = true;
        }
    }

    private saveState() {
        let vsctx = this._context.vscodeExtensionContext;
        vsctx.globalState.update("azALDevTools.remETrig.removeTriggers", !!this._settings.removeTriggers);
        vsctx.globalState.update("azALDevTools.remETrig.removeSubscribers", !!this._settings.removeSubscribers);
    }

    private clearSettings() {
        this._settings.removeTriggers = false;
        this._settings.removeSubscribers = false;
    }

}