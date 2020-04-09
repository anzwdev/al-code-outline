import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';

export class ALCodeCommand {    
    protected _toolsExtensionContext : DevToolsExtensionContext;        
    public name: string;
    
    constructor(context : DevToolsExtensionContext, commandName: string) {
        this._toolsExtensionContext = context;
        this.name = commandName;
        this._toolsExtensionContext.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                commandName,
                () => this.run()
            ));        
    }

    collectCodeActions(symbol: AZSymbolInformation, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
    }

    protected run() {
        if (!vscode.window.activeTextEditor)
            return;
        let position = vscode.window.activeTextEditor.selection.active;
        let range = new vscode.Range(position, position);
        this.runAsync(range);        
    }

    protected async runAsync(range: vscode.Range) {
    }

    protected getDocumentUri() : vscode.Uri {
        return this._toolsExtensionContext.activeDocumentSymbols.getDocUri();
    }

    protected canRunOnSave(actionCode: string, resource?: vscode.Uri) {
        let actionsList = vscode.workspace.getConfiguration('alOutline', resource).get<string[]>('codeActionsOnSave');
        return (actionsList.indexOf(actionCode) >= 0);
    }

}