import * as vscode from 'vscode';
import { ALCodeCommand } from "./alCodeCommand";
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALSymbolsBasedInterfaceWizard } from '../objectwizards/symbolwizards/alSymbolsBasedInterfaceWizard';

export class ALCreateInterfaceCodeCommand extends ALCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALCreateInterfaceCodeCommand');
    }

    collectCodeActions(symbol: AZSymbolInformation, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) && (symbol.kind == AZSymbolKind.CodeunitObject) && (symbol.selectionRange.start.line == range.start.line)) {
            let action = new vscode.CodeAction("Create interface", vscode.CodeActionKind.QuickFix);
            action.command = { command: this.name, title: 'Create interface...' };
            actions.push(action);
        }
    }

    protected async runAsync(range: vscode.Range) {
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if ((!symbol) || (symbol.kind != AZSymbolKind.CodeunitObject)) {
            return;
        }
        let wizard = new ALSymbolsBasedInterfaceWizard(this._toolsExtensionContext);
        await wizard.showInterfaceWizard(symbol);
    }

}