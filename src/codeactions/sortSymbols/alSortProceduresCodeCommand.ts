import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { ALBaseSortCodeCommand } from './alBaseSortCodeCommand';

export class ALSortProceduresCodeCommand extends ALBaseSortCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALSortProceduresCodeCommand');
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

    protected async runAsync(range: vscode.Range) {
        // Get required details from document source code
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if (!symbol) {
            return;
        }
        const methodSymbolKinds: AZSymbolKind[] = ALSortProceduresCodeCommand.getMethodSymbolKinds();

        let parentKind: AZSymbolKind[] = [AZSymbolKind.CodeunitObject, AZSymbolKind.PageObject, AZSymbolKind.ReportObject, AZSymbolKind.QueryObject, AZSymbolKind.XmlPortObject, AZSymbolKind.TableExtensionObject, AZSymbolKind.PageExtensionObject];
        let objectSymbol = symbol.findParentByKindList(parentKind);
        let isMethodSymbol = methodSymbolKinds.indexOf(symbol.kind) > -1;

        if ((!objectSymbol) || 
            ((!isMethodSymbol) && (!symbol.contentRange)) || 
            ((isMethodSymbol) && (!symbol.range))
        )
        {
            return;
        }
        

        // Collect method declarations (of matching symbol-kind)
        let methodDecls: AZSymbolInformation[] = [];
        objectSymbol.collectChildSymbols(symbol.kind, methodDecls);
        if (methodDecls.length == 0) {
            return;
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
        let insertPos: vscode.Position = new vscode.Position(methodDecls[0].range.start.line, methodDecls[0].range.start.character);
        await vscode.window.activeTextEditor.edit(editBuilder => {
            for (const methodDecl of methodDecls) {
                const deleteRange = new vscode.Range(methodDecl.range.start.line, methodDecl.range.start.character, methodDecl.range.end.line, methodDecl.range.end.character);
                editBuilder.delete(deleteRange);
            }
            
            editBuilder.insert(insertPos, newSource);
        });
    }
}
