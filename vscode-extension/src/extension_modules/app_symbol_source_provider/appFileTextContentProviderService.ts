import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { AppFileTextContentProvider } from "./appFileTextContentProvider";
import { AppFileTextContentProviderConst } from './appFileTextContentProviderConst';

export class AppFileTextContentProviderService extends DevToolsExtensionService {

    private _appFileTextContentProvider: AppFileTextContentProvider;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._appFileTextContentProvider = new AppFileTextContentProvider(context);
        this.subscriptions.push(vscode.workspace.registerTextDocumentContentProvider(AppFileTextContentProviderConst.scheme, this._appFileTextContentProvider));
    }

}

