import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NameValueQuickPickItem } from '../tools/nameValueQuickPickItem';
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';

export class PageControlsCaptionsModifier extends WorkspaceCommandSyntaxModifier {
    protected _controlTypes: any;

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add Page Control Captions", "addPageControlCaptions");
        this._controlTypes = {};
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.setActionsCaptions = this._controlTypes.setActionsCaptions;
        parameters.setActionGroupsCaptions = this._controlTypes.setActionGroupsCaptions;
        parameters.setGroupsCaptions = this._controlTypes.setGroupsCaptions;
        parameters.setPartsCaptions = this._controlTypes.setPartsCaptions;
        parameters.setFieldsCaptions = this._controlTypes.setFieldsCaptions;
        parameters.setLabelsCaptions = this._controlTypes.setLabelsCaptions;

        return parameters;
    }

    async askForParameters(uri: vscode.Uri | undefined): Promise<boolean> {
        this.loadState();
        let quickPickItems = [
            new NameValueQuickPickItem('Page actions', 'setActionsCaptions', this._controlTypes.setActionsCaptions),
            new NameValueQuickPickItem('Page action groups', 'setActionGroupsCaptions', this._controlTypes.setActionGroupsCaptions),
            new NameValueQuickPickItem('Page groups', 'setGroupsCaptions', this._controlTypes.setGroupsCaptions),
            new NameValueQuickPickItem('Page parts', 'setPartsCaptions', this._controlTypes.setPartsCaptions),
            new NameValueQuickPickItem('Page fields', 'setFieldsCaptions', this._controlTypes.setFieldsCaptions),
            new NameValueQuickPickItem('Page labels', 'setLabelsCaptions', this._controlTypes.setLabelsCaptions)
        ];

        let selectedValues = await vscode.window.showQuickPick(
            quickPickItems, { canPickMany: true, placeHolder: 'Select page controls to update' });

        if (!selectedValues)
            return false;

        this.clearControlTypes();
        if (selectedValues) {
            for (let i=0; i<selectedValues.length; i++) {
                this._controlTypes[selectedValues[i].value] = true;
            }
        }
        this.saveState();

        return true;
    }

    private loadState() {
        this._controlTypes = {};
        let vsctx = this._context.vscodeExtensionContext;
        this._controlTypes.setActionsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setActionsCaptions");
        this._controlTypes.setActionGroupsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setActionGroupsCaptions");
        this._controlTypes.setGroupsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setGroupsCaptions");
        this._controlTypes.setPartsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setPartsCaptions");
        this._controlTypes.setFieldsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setFieldsCaptions");
        this._controlTypes.setLabelsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setLabelsCaptions");
        //set defaults
        if ((!this._controlTypes.setActionsCaptions) &&
            (!this._controlTypes.setActionGroupsCaptions) &&
            (!this._controlTypes.setGroupsCaptions) &&
            (!this._controlTypes.setPartsCaptions) &&
            (!this._controlTypes.setFieldsCaptions) &&
            (!this._controlTypes.setLabelsCaptions)) {

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