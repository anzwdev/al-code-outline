import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextRange } from '../symbollibraries/textRange';
import { SyntaxModifier } from "./syntaxModifier";

export class OnDocumentSaveModifier extends SyntaxModifier {
    protected _commandsList: string | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context, "runMultiple");
        this._context = context;
        this._commandsList = undefined;
    }

    private getCommandsList(uri: vscode.Uri) {
        this._commandsList = '';
        let actionsList = vscode.workspace.getConfiguration('alOutline', uri).get<string[]>('codeActionsOnSave');
        if ((actionsList) && (actionsList.length > 0)) {
            for (let i=0; i<actionsList.length; i++) {
                let name = actionsList[i];
                name = name.substr(0,1).toLowerCase() + name.substr(1);
                if (i > 0)
                    this._commandsList = this._commandsList + ',' + name;
                else
                    this._commandsList = name;
            }
        }
    }

    protected getParameters(uri: vscode.Uri): any {
        return {
            commandsList: this._commandsList,
            skipFormatting: 'true'
        }
    }

    async RunForDocument(document: vscode.TextDocument, range: TextRange | undefined, withUI: boolean) {
        this.getCommandsList(document.uri);
        if (this._commandsList)
            await super.RunForDocument(document, range, withUI);
    }
}