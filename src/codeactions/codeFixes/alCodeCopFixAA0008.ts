import * as vscode from 'vscode';
import { ALCodeFix } from "../alCodeFix";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALCodeCopFixAA0008 extends ALCodeFix {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AA0008");
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction | undefined {
        let settings =  vscode.workspace.getConfiguration('alOutline', document.uri);
        let fixOnSave = settings.get<boolean>('fixCodeCopMissingParenthesesOnSave');      
        if (!fixOnSave)
            fixOnSave = false;  

        const fix = new vscode.CodeAction(`Add parentheses`, this.getCodeActionKind(fixOnSave));
            
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        fix.edit.insert(document.uri, new vscode.Position(currRange.end.line, currRange.end.character), '()');
        fix.isPreferred = true;
        return fix;
    }

}
