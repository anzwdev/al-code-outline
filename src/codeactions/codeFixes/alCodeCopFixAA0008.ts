import * as vscode from 'vscode';
import { ALCodeFix } from "../alCodeFix";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';

export class ALCodeCopFixAA0008 extends ALCodeFix {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AA0008");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        let settings =  vscode.workspace.getConfiguration('alOutline', document.uri);
        let fixOnSave = settings.get<boolean>('fixCodeCopMissingParenthesesOnSave');      
        if (fixOnSave) {
            let onSaveEdit: vscode.WorkspaceEdit | undefined = undefined;
            for (let i=0; i<context.diagnostics.length; i++) {
                if (context.diagnostics[i].code === this.diagnosticCode) {
                    if (!onSaveEdit)
                        onSaveEdit = new vscode.WorkspaceEdit();
                    onSaveEdit.insert(document.uri, context.diagnostics[i].range.end, '()');
                }
            }
            if (onSaveEdit) {
                let actionKind = vscode.CodeActionKind.SourceFixAll; //.append('al');
                let action = new vscode.CodeAction("Fix missing parentheses", actionKind);
                action.edit = onSaveEdit;
                actions.push(action);
            }
        }
    }

}
