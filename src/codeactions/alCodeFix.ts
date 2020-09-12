import * as vscode from 'vscode';
import { ALCodeAction } from './alCodeAction';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZDocumentSymbolsLibrary } from '../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';

export class ALCodeFix extends ALCodeAction {
    diagnosticCode: string;

    constructor(context : DevToolsExtensionContext, diagCode: string) {
        super(context, diagCode);
        this.diagnosticCode = diagCode;
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[], onSaveEdit: vscode.WorkspaceEdit | undefined): vscode.WorkspaceEdit | undefined {
        for (let i=0; i<context.diagnostics.length; i++) {
            if (context.diagnostics[i].code === this.diagnosticCode) {
                let codeFix = this.createFix(document, context.diagnostics[i]);
                if (codeFix)
                    actions.push(codeFix);
            }
        }
        return onSaveEdit;
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction | undefined {
        return undefined;
    }

}