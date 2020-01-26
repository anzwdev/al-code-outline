import * as vscode from 'vscode';
import { ALCodeFix } from "./alCodeFix";

export class ALCodeCopFixAA0008 extends ALCodeFix {

    constructor() {
        super();
        this.diagnosticCode = "AA0008";
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        const fix = new vscode.CodeAction(`Add parentheses`, vscode.CodeActionKind.QuickFix);
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        fix.edit.insert(document.uri, new vscode.Position(currRange.end.line, currRange.end.character), '()');
        fix.isPreferred = true;
        return fix;
    }

}