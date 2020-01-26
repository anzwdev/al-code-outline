import * as vscode from 'vscode';
import { ALCodeFix } from "./alCodeFix";

export class ALCodeCopFixAA0137 extends ALCodeFix {

    constructor() {
        super();
        this.diagnosticCode = "AA0137";
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        const fix = new vscode.CodeAction(`Remove Variable`, vscode.CodeActionKind.QuickFix);
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        let startPosition = new vscode.Position(currRange.start.line, currRange.start.character);
        let stopPosition = new vscode.Position(currRange.end.line + 1, 0)
        fix.edit.replace(document.uri, new vscode.Range(startPosition, stopPosition), "");
        fix.isPreferred = true;
        return fix;
    }

}