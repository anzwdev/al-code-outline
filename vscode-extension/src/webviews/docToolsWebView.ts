import * as vscode from 'vscode';
import * as path from 'path';
import { BaseWebViewEditor } from "./baseWebViewEditor";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

export class DocToolsWebView extends BaseWebViewEditor {
    protected _devToolsContext : DevToolsExtensionContext;
    protected _loaded: boolean;
    protected _documentUri: vscode.Uri | undefined;

    constructor(devToolsContext : DevToolsExtensionContext, documentName: string | undefined, documentUri: vscode.Uri | undefined) {        
        if ((!documentName) && (documentUri))
            documentName = path.parse(documentUri.path).base;
           
        super(devToolsContext.vscodeExtensionContext, documentName);
        
        this._documentUri = documentUri;
        this._devToolsContext = devToolsContext;
        this._loaded = false;
        this._viewColumn = vscode.ViewColumn.Beside;
    }

    

}