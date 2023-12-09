import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALCodeAction } from './alCodeAction';

export class ALAddPermissionsCodeCommand extends ALCodeAction {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AddAllObjectsPermissions");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        let edit: vscode.WorkspaceEdit | undefined =  undefined;

        //collect list of objects in selection range
        if ((symbol) && 
            (symbol.selectionRange) &&
            (symbol.selectionRange.start.line == range.start.line)) {

            let isPermissionsNode = (symbol.kind == AZSymbolKind.Property) && 
                (symbol.name) && 
                (symbol.name.toLowerCase() == "permissions") && 
                (!symbol.containsDiagnostics);
            let isALObject = symbol.isALObject();

            if (isPermissionsNode || isALObject) {
                let appObject = symbol.findParentObject();
                if (appObject) {
                    switch (appObject.kind) {
                        case AZSymbolKind.PermissionSet:
                        case AZSymbolKind.PermissionSetExtension:
                            let allObjectsAction = new vscode.CodeAction("Add all extension objects permissions (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                            allObjectsAction.command = {
                                command: "azALDevTools.addAllObjectsPermissions",
                                title: "Add All Extension Objects Permissions",
                                arguments: [document, symbol.range]
                            }
                            actions.push(allObjectsAction);
                            break;
                        case AZSymbolKind.CodeunitObject:
                        case AZSymbolKind.ReportObject:
                        case AZSymbolKind.PageObject:
                        case AZSymbolKind.XmlPortObject:
                        case AZSymbolKind.QueryObject:
                        case AZSymbolKind.TableObject:
                            let usedObjectsAction = new vscode.CodeAction("Add permissions to all tables used by this object (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
                            usedObjectsAction.command = {
                                command: "azALDevTools.addReferencedTablesPermissions",
                                title: "Add Used Tables Permissions",
                                arguments: [document, appObject.range]
                            };
                            actions.push(usedObjectsAction);
                            break;
                    }
                }
            }
        }
    }

}