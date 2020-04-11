import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { ALCodeAction } from './alCodeAction';

export class ALCodeCommand extends ALCodeAction {    
    public name: string;
    
    constructor(context : DevToolsExtensionContext, commandName: string) {
        super(context);
        this.name = commandName;
        this._toolsExtensionContext.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                commandName,
                (docSymbols, document, range) => this.runAsync(docSymbols, document, range)
            ));        
    }

    protected async runAsync(docSymbols: AZDocumentSymbolsLibrary, document: vscode.TextDocument, range: vscode.Range) {
    }

}