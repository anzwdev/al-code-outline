import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from "./baseWebViewEditor";
import { DevToolsExtensionContext } from '../devToolsExtensionContext';

export class DocToolsWebView extends BaseWebViewEditor {
    protected _loaded: boolean;
    protected _documentUri: vscode.Uri | undefined;

    constructor(context : DevToolsExtensionContext, documentName: string | undefined, documentUri: vscode.Uri | undefined, viewType: string) {        
        if ((!documentName) && (documentUri)) {
            documentName = path.parse(documentUri.path).base;
        }
           
        super(context, documentName, viewType);
        
        this._documentUri = documentUri;
        this._loaded = false;
        this._viewColumn = vscode.ViewColumn.Beside;
    }

}