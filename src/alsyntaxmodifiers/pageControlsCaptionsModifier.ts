import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NameValueQuickPickItem } from '../tools/nameValueQuickPickItem';
import { IAddPageFieldCaptionsSettings } from './iAddPageFieldCaptionsSettings';
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class PageControlsCaptionsModifier extends WorkspaceCommandSyntaxModifier {
    protected _controlTypes: IAddPageFieldCaptionsSettings;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Page Control Captions", "addPageControlCaptions");
        this._controlTypes = {
            setActionsCaptions: false,
            setActionGroupsCaptions: false,
            setGroupsCaptions: false,
            setPartsCaptions: false,
            setFieldsCaptions: false,
            setLabelsCaptions: false           
        };
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        this.copySettings(parameters, this._controlTypes)
        return parameters;
    }

    protected copySettings(dest: any, src: any) {
        if (!src)
            src = {};
        dest.setActionsCaptions = !!src.setActionsCaptions;
        dest.setActionGroupsCaptions = !!src.setActionGroupsCaptions;
        dest.setGroupsCaptions = !!src.setGroupsCaptions;
        dest.setPartsCaptions = !!src.setPartsCaptions;
        dest.setFieldsCaptions = !!src.setFieldsCaptions;
        dest.setLabelsCaptions = !!src.setLabelsCaptions;
    }

    protected areSettingsEmpty(value: IAddPageFieldCaptionsSettings | undefined) {
        return ((!value) || (
            (!value.setActionsCaptions) &&
            (!value.setActionGroupsCaptions) &&
            (!value.setGroupsCaptions) &&
            (!value.setPartsCaptions) &&
            (!value.setFieldsCaptions) &&
            (!value.setLabelsCaptions)));
    }

    protected loadDefaultParameters(uri: vscode.Uri | undefined): boolean {
        let defaultParameters = vscode.workspace.getConfiguration('alOutline', uri).get<IAddPageFieldCaptionsSettings>('defaultAddPageFieldCaptionsSettings');
        if (this.areSettingsEmpty(defaultParameters))
            return false;
        this.copySettings(this._controlTypes, defaultParameters);
        return true;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        this.loadState();
        let quickPickItems = [
            new NameValueQuickPickItem('Page actions', 'setActionsCaptions', !!this._controlTypes.setActionsCaptions),
            new NameValueQuickPickItem('Page action groups', 'setActionGroupsCaptions', !!this._controlTypes.setActionGroupsCaptions),
            new NameValueQuickPickItem('Page groups', 'setGroupsCaptions', !!this._controlTypes.setGroupsCaptions),
            new NameValueQuickPickItem('Page parts', 'setPartsCaptions', !!this._controlTypes.setPartsCaptions),
            new NameValueQuickPickItem('Page fields', 'setFieldsCaptions', !!this._controlTypes.setFieldsCaptions),
            new NameValueQuickPickItem('Page labels', 'setLabelsCaptions', !!this._controlTypes.setLabelsCaptions)
        ];

        let selectedValues = await vscode.window.showQuickPick(
            quickPickItems, { canPickMany: true, placeHolder: 'Select page controls to update' });

        if (!selectedValues)
            return false;

        let data: any = {};
        if (selectedValues) {
            for (let i=0; i<selectedValues.length; i++) {
                data[selectedValues[i].value] = true;
            }
        }
        if (this.areSettingsEmpty(data))
            return false;
        this.copySettings(this._controlTypes, data);
        this.saveState();

        return true;
    }

    private loadState() {
        this.clearControlTypes();
        let vsctx = this._context.vscodeExtensionContext;
        this._controlTypes.setActionsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setActionsCaptions");
        this._controlTypes.setActionGroupsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setActionGroupsCaptions");
        this._controlTypes.setGroupsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setGroupsCaptions");
        this._controlTypes.setPartsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setPartsCaptions");
        this._controlTypes.setFieldsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setFieldsCaptions");
        this._controlTypes.setLabelsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setLabelsCaptions");
        //set defaults
        if (this.areSettingsEmpty(this._controlTypes)) {
            this._controlTypes.setActionsCaptions = true;
            this._controlTypes.setActionGroupsCaptions = true;
            this._controlTypes.setGroupsCaptions = true;
            this._controlTypes.setPartsCaptions = false;
            this._controlTypes.setFieldsCaptions = false;
            this._controlTypes.setLabelsCaptions = false;
        }
    }

    private saveState() {
        let vsctx = this._context.vscodeExtensionContext;
        vsctx.globalState.update("azALDevTools.setPgCap.setActionsCaptions", this._controlTypes.setActionsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setActionGroupsCaptions", this._controlTypes.setActionGroupsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setGroupsCaptions", this._controlTypes.setGroupsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setPartsCaptions", this._controlTypes.setPartsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setFieldsCaptions", this._controlTypes.setFieldsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setLabelsCaptions", this._controlTypes.setLabelsCaptions);
    }

    private clearControlTypes() {
        this._controlTypes.setActionsCaptions = false;
        this._controlTypes.setActionGroupsCaptions = false;
        this._controlTypes.setGroupsCaptions = false;
        this._controlTypes.setPartsCaptions = false;
        this._controlTypes.setFieldsCaptions = false;
        this._controlTypes.setLabelsCaptions = false;
    }

}