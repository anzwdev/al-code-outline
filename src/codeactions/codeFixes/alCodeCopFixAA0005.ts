import * as vscode from 'vscode';
import { ALCodeFix } from "../alCodeFix";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALCodeCopFixAA0005 extends ALCodeFix {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AA0005");
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction | undefined {
        const fix = new vscode.CodeAction(`Remove begin..end (AZ AL Dev Tools)`, vscode.CodeActionKind.QuickFix);
        fix.edit = new vscode.WorkspaceEdit();

        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        let text = document.getText(diagnostic.range);
        let last = text.length;
        if (text[text.length - 1] == ';' || text[text.length - 1] === ';') {
            last = text.length - 4;
        }
        else {
            last = text.length - 3;
        }
        text = text.slice(5, last)
        fix.edit.replace(document.uri, currRange, text);

        fix.isPreferred = true;
        return fix;
    }

}