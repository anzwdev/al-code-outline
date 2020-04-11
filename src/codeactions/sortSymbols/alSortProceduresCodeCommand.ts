import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { ALBaseSortCodeCommand } from './alBaseSortCodeCommand';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALSortProceduresCodeCommand extends ALBaseSortCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context);
    }

    public static getMethodSymbolKinds(): AZSymbolKind[] {
        return [
                AZSymbolKind.MethodDeclaration, 
                AZSymbolKind.LocalMethodDeclaration, 
                AZSymbolKind.EventSubscriberDeclaration,
                AZSymbolKind.EventDeclaration,
                AZSymbolKind.BusinessEventDeclaration,
                AZSymbolKind.IntegrationEventDeclaration
        ];
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) && (ALSortProceduresCodeCommand.getMethodSymbolKinds().indexOf(symbol.kind) > -1 && symbol.selectionRange.start.line == range.start.line)) {
            let edit = this.prepareEdit(symbol, document);
            if (edit) {
                let action = new vscode.CodeAction("Sort procedures", vscode.CodeActionKind.QuickFix);
                action.edit = edit;
                actions.push(action);
            }
        }
    }

    protected prepareEdit(symbol: AZSymbolInformation, document: vscode.TextDocument): vscode.WorkspaceEdit | undefined {
        const methodSymbolKinds: AZSymbolKind[] = ALSortProceduresCodeCommand.getMethodSymbolKinds();

        let parentKind: AZSymbolKind[] = [AZSymbolKind.CodeunitObject, AZSymbolKind.PageObject, AZSymbolKind.ReportObject, AZSymbolKind.QueryObject, AZSymbolKind.XmlPortObject, AZSymbolKind.TableExtensionObject, AZSymbolKind.PageExtensionObject];
        let objectSymbol = symbol.findParentByKindList(parentKind);
        let isMethodSymbol = methodSymbolKinds.indexOf(symbol.kind) > -1;

        if ((!objectSymbol) || 
            ((!isMethodSymbol) && (!symbol.contentRange)) || 
            ((isMethodSymbol) && (!symbol.range))
        )
        {
            return undefined;
        }
        
        // Collect method declarations (of matching symbol-kind)
        let methodDecls: AZSymbolInformation[] = [];
        objectSymbol.collectChildSymbols(symbol.kind, true, methodDecls);
        if (methodDecls.length == 0) {
            return undefined;
        }

        // Sort the method declarations
        methodDecls.sort((methodDeclA, methodDeclB) => {
            return methodDeclA.name.localeCompare(methodDeclB.name, undefined, { numeric: true, sensitivity: 'base' });
        });

        // Produce the new sorted source
        let newSource: string = "";
        for (const methodDecl of methodDecls) {
            const declRange = new vscode.Range(methodDecl.range.start.line, methodDecl.range.start.character, methodDecl.range.end.line, methodDecl.range.end.character);
            newSource += vscode.window.activeTextEditor.document.getText(declRange);
        }
        
        // Delete the old unsorted method declarations and insert the new sorted source
        let edit = new vscode.WorkspaceEdit();
        let insertPos: vscode.Position = new vscode.Position(methodDecls[0].range.start.line, methodDecls[0].range.start.character);
            
        for (const methodDecl of methodDecls) {
            const deleteRange = new vscode.Range(methodDecl.range.start.line, methodDecl.range.start.character, methodDecl.range.end.line, methodDecl.range.end.character);
            edit.delete(document.uri, deleteRange);
        }
            
        edit.insert(document.uri, insertPos, newSource);

        return edit;
    }
}
