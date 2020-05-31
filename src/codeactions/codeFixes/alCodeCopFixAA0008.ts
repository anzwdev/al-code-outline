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
        if (!fixOnSave)
            return;  

        let edit: vscode.WorkspaceEdit | undefined;

        for (let i=0; i<context.diagnostics.length; i++) {
            if (context.diagnostics[i].code === this.diagnosticCode) {
                if (!edit)
                    edit = new vscode.WorkspaceEdit();
                edit.insert(document.uri, context.diagnostics[i].range.end, '()');
            }
        }

        if (edit) {
            let fix = new vscode.CodeAction(`Add parenthesis`, this.getCodeActionKind(fixOnSave));
            fix.edit = edit;
            fix.isPreferred = true;
            actions.push(fix);
        }

    }

}
