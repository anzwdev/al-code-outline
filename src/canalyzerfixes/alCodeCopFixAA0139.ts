import * as vscode from 'vscode';
import { ALCodeFix } from "./alCodeFix";

export class ALCodeCopFixAA0139 extends ALCodeFix {

    constructor() {
        super();
        this.diagnosticCode = "AA0139";
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        const fix = new vscode.CodeAction(`Add CopyStr`, vscode.CodeActionKind.QuickFix);
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        let startPosition = new vscode.Position(currRange.start.line, 0);
        let stopPosition = new vscode.Position(currRange.end.line, currRange.end.character)
        let text = document.getText(new vscode.Range(startPosition, stopPosition));
        if (text.match(/(:=)/)) {
            let split = text.split(":=");
            let right = split[0].trim();
            let stmt = "CopyStr(" + document.getText(currRange) + ",1,MaxStrLen(" + right + "))";
            fix.edit.replace(document.uri, currRange, stmt);

            fix.isPreferred = true;
            return fix;
        } else
            return undefined;
    }

}