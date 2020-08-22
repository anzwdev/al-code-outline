import * as vscode from 'vscode';
import { ALCodeCommand } from "../alCodeCommand";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSymbolsBasedInterfaceWizard } from '../../objectwizards/symbolwizards/alSymbolsBasedInterfaceWizard';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALCreateInterfaceCodeCommand extends ALCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'CreateInterface', 'AZDevTools.ALCreateInterfaceCodeCommand');
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[], onSaveEdit: vscode.WorkspaceEdit | undefined): vscode.WorkspaceEdit | undefined {
        if ((symbol) && 
            (symbol.kind == AZSymbolKind.CodeunitObject) && 
            (symbol.selectionRange) &&
            (symbol.selectionRange.start.line == range.start.line)) {
            let action = new vscode.CodeAction("Create interface", vscode.CodeActionKind.QuickFix);
            action.command = { 
                command: this.name, 
                title: 'Create interface...',
                arguments: [docSymbols, document, range]
            };
            actions.push(action);
        }

        return onSaveEdit;
    }

    protected async runAsync(docSymbols: AZDocumentSymbolsLibrary, document: vscode.TextDocument, range: vscode.Range) {
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if ((!symbol) || (symbol.kind != AZSymbolKind.CodeunitObject)) {
            return;
        }
        let wizard = new ALSymbolsBasedInterfaceWizard(this._toolsExtensionContext);
        await wizard.showInterfaceWizard(symbol);
    }

}