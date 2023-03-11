import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALHoverProvider } from "../editorextensions/alHoverProvider";
import { DevToolsExtensionService } from "./devToolsExtensionService";

export class HoverService extends DevToolsExtensionService {

    protected _alHoverProvider: ALHoverProvider;
    
    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._alHoverProvider = new ALHoverProvider(context);
        vscode.languages.registerHoverProvider('al', this._alHoverProvider);
    }

}
