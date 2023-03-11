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

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        for (let i=0; i<diagnostics.length; i++) {
            let diagCode: any = diagnostics[i].code;
            if ((diagCode) && ((diagCode === this.diagnosticCode) || (diagCode.value === this.diagnosticCode))) {
                let codeFix = this.createFix(document, diagnostics[i]);
                if (codeFix)
                    actions.push(codeFix);
            }
        }
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction | undefined {
        return undefined;
    }

}