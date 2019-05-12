import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALAddPageFieldsCodeCommand } from './alAddPageFiedsCodeCommand';
import { ALAddQueryFieldsCodeCommand } from './alAddQueryFieldsCodeCommand';
import { ALAddReportFieldsCodeCommand } from './alAddReportFieldsCodeCommand';

export class ALCodeActionsProvider implements vscode.CodeActionProvider {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _addPageFieldsCommand : ALAddPageFieldsCodeCommand;
    protected _addQueryFieldsCommand : ALAddQueryFieldsCodeCommand;
    protected _addReportFieldsCommand: ALAddReportFieldsCodeCommand;

    constructor(context : DevToolsExtensionContext) {
        this._toolsExtensionContext = context;
        this._addPageFieldsCommand = new ALAddPageFieldsCodeCommand(this._toolsExtensionContext);
        this._addQueryFieldsCommand = new ALAddQueryFieldsCodeCommand(this._toolsExtensionContext);
        this._addReportFieldsCommand = new ALAddReportFieldsCodeCommand(this._toolsExtensionContext);
    }

    provideCodeActions(document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, token: vscode.CancellationToken): vscode.ProviderResult<(vscode.Command | vscode.CodeAction)[]> {
        let actions: vscode.CodeAction[] = [];

        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if (symbol) {
            //add multiple fields to a page
            if ((symbol.kind == AZSymbolKind.PageGroup) ||                 
                (symbol.kind == AZSymbolKind.PageRepeater) ||
                (symbol.kind == AZSymbolKind.ControlAddChange)) {                
                let action : vscode.CodeAction = new vscode.CodeAction("Add multiple fields", vscode.CodeActionKind.QuickFix);
                action.command = { command: this._addPageFieldsCommand.name, title: 'Add multiple fields...' };
                actions.push(action);
            }
            //add multiple fields to a query
            if (symbol.kind == AZSymbolKind.QueryDataItem) {
                let action = new vscode.CodeAction("Add multiple fields", vscode.CodeActionKind.QuickFix);
                action.command = { command: this._addQueryFieldsCommand.name, title: 'Add multiple fields...' };
                actions.push(action);
            }
            //add multiple fields to a report
            if (symbol.kind == AZSymbolKind.ReportDataItem) {
                let action = new vscode.CodeAction("Add multiple fields", vscode.CodeActionKind.QuickFix);
                action.command = { command: this._addReportFieldsCommand.name, title: 'Add multiple fields...' };
                actions.push(action);
            }
        }
        
        return actions;
    }

}