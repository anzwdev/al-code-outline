import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ALReferencesProvider } from '../editorextensions/alReferencesProvider';
import { DevToolsExtensionService } from "./devToolsExtensionService";

export class ReferencesService extends DevToolsExtensionService {

    protected _alReferencesProvider: ALReferencesProvider;
    
    constructor(context: DevToolsExtensionContext) {
        super(context);

        this._alReferencesProvider = new ALReferencesProvider(context);
        vscode.languages.registerReferenceProvider('al', this._alReferencesProvider);
    }

}
