import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { CodeOutlineWebViewTreeProvider } from "./codeOutlineWebViewTreeProvider";

export class SourceCodeOutlineWebViewTreeProvider extends CodeOutlineWebViewTreeProvider{

    constructor(context: DevToolsExtensionContext, documentUri?: vscode.Uri, documentName?: string) {
        super(context, documentUri, documentName);
    }

}