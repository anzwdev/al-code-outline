import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NameValueQuickPickItem } from '../tools/nameValueQuickPickItem';
import { SyntaxModifier } from "./syntaxModifier";

export class PageControlsCaptionsModifier extends SyntaxModifier {
    protected _controlTypes: any;

    constructor(context: DevToolsExtensionContext) {
        super(context, "addPageControlCaptions");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
        this._controlTypes = {};
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.setActionsCaptions = this._controlTypes.setActionsCaptions;
        parameters.setActionGroupsCaptions = this._controlTypes.setActionGroupsCaptions;
        parameters.setGroupsCaptions = this._controlTypes.setGroupsCaptions;
        parameters.setPartsCaptions = this._controlTypes.setPartsCaptions;
        parameters.setFieldsCaptions = this._controlTypes.setFieldsCaptions;

        return parameters;
    }

    protected async askForParameters() {
        this.loadState();
        let quickPickItems = [
            new NameValueQuickPickItem('Page actions', 'setActionsCaptions', this._controlTypes.setActionsCaptions),
            new NameValueQuickPickItem('Page action groups', 'setActionGroupsCaptions', this._controlTypes.setActionGroupsCaptions),
            new NameValueQuickPickItem('Page groups', 'setGroupsCaptions', this._controlTypes.setGroupsCaptions),
            new NameValueQuickPickItem('Page parts', 'setPartsCaptions', this._controlTypes.setPartsCaptions),
            new NameValueQuickPickItem('Page fields', 'setFieldsCaptions', this._controlTypes.setFieldsCaptions)
        ];

        let selectedValues = await vscode.window.showQuickPick(
            quickPickItems, { canPickMany: true, placeHolder: 'Select page controls to update' });

        this.clearControlTypes();
        if (selectedValues) {
            for (let i=0; i<selectedValues.length; i++) {
                this._controlTypes[selectedValues[i].value] = true;
            }
        }
        this.saveState();
    }

    private loadState() {
        this._controlTypes = {};
        let vsctx = this._context.vscodeExtensionContext;
        this._controlTypes.setActionsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setActionsCaptions");
        this._controlTypes.setActionGroupsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setActionGroupsCaptions");
        this._controlTypes.setGroupsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setGroupsCaptions");
        this._controlTypes.setPartsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setPartsCaptions");
        this._controlTypes.setFieldsCaptions = !!vsctx.globalState.get<boolean>("azALDevTools.setPgCap.setFieldsCaptions");
        //set defaults
        if ((!this._controlTypes.setActionsCaptions) &&
            (!this._controlTypes.setActionGroupsCaptions) &&
            (!this._controlTypes.setGroupsCaptions) &&
            (!this._controlTypes.setPartsCaptions) &&
            (!this._controlTypes.setFieldsCaptions)) {

            this._controlTypes.setActionsCaptions = true;
            this._controlTypes.setActionGroupsCaptions = true;
            this._controlTypes.setGroupsCaptions = true;
            this._controlTypes.setPartsCaptions = false;
            this._controlTypes.setFieldsCaptions = false;
        }
    }

    private saveState() {
        let vsctx = this._context.vscodeExtensionContext;
        vsctx.globalState.update("azALDevTools.setPgCap.setActionsCaptions", this._controlTypes.setActionsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setActionGroupsCaptions", this._controlTypes.setActionGroupsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setGroupsCaptions", this._controlTypes.setGroupsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setPartsCaptions", this._controlTypes.setPartsCaptions);
        vsctx.globalState.update("azALDevTools.setPgCap.setFieldsCaptions", this._controlTypes.setFieldsCaptions);
    }

    private clearControlTypes() {
        this._controlTypes.setActionsCaptions = false;
        this._controlTypes.setActionGroupsCaptions = false;
        this._controlTypes.setGroupsCaptions = false;
        this._controlTypes.setPartsCaptions = false;
        this._controlTypes.setFieldsCaptions = false;
    }

}