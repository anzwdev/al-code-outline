import * as vscode from 'vscode';
import { ALCodeFix } from "../alCodeFix";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';

export class ALCodeCopFixAA0008 extends ALCodeFix {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AA0008");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        let settings =  vscode.workspace.getConfiguration('alOutline', document.uri);
        let fixOnSave: boolean = !!settings.get<boolean>('fixCodeCopMissingParenthesesOnSave');

        if (fixOnSave) {
            let onSaveEdit: vscode.WorkspaceEdit | undefined = undefined;
            for (let i=0; i<diagnostics.length; i++) {
                let diagCode: any = diagnostics[i].code;
                if ((diagCode) && ((diagCode == this.diagnosticCode) || (diagCode.value == this.diagnosticCode))) {
                    if (!onSaveEdit)
                        onSaveEdit = new vscode.WorkspaceEdit();
                    onSaveEdit.insert(document.uri, diagnostics[i].range.end, '()');
                }
            }
            if (onSaveEdit) {
                let actionKind = vscode.CodeActionKind.SourceFixAll.append('al');
                let action = new vscode.CodeAction("Fix missing parentheses (AZ AL Dev Tools)", actionKind);
                action.edit = onSaveEdit;
                actions.push(action);
            }
        }
    }

}
