import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALCodeAction } from './alCodeAction';

export class ALAddAllPermissionsCodeCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AddAllObjectsPermissions");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        let edit: vscode.WorkspaceEdit | undefined =  undefined;

        //collect list of objects in selection range
        if ((symbol) && 
            (symbol.kind == AZSymbolKind.Property) && 
            (symbol.name) && 
            (symbol.name.toLowerCase() == "permissions") &&
            (!symbol.containsDiagnostics) &&
            (symbol.selectionRange) &&
            (symbol.selectionRange.start.line == range.start.line)) {

            let action = new vscode.CodeAction("Add all extension objects permissions (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
            action.command = {
                command: "azALDevTools.addAllObjectsPermissions",
                title: "Add All Extension Objects Permissions",
                arguments: [document, symbol.range]
            }
            actions.push(action);
        }
    }

}