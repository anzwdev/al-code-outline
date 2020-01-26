import * as vscode from 'vscode';

export class ALCodeFix {
    diagnosticCode: string;

    constructor() {
        this.diagnosticCode = "";
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        return new vscode.CodeAction('');
    }

}