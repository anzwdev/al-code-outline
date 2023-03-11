import * as vscode from 'vscode';
import { ALCodeFix } from "../alCodeFix";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { TextRange } from '../../symbollibraries/textRange';

export class ALCodeCopFixAA0137 extends ALCodeFix {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AA0137");
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction | undefined {
        const fix = new vscode.CodeAction(`Remove Variable (AZ AL Dev Tools)`, vscode.CodeActionKind.QuickFix);

        let fixRange = TextRange.fromAny(diagnostic.range)

        fix.command = {
            command: "azALDevTools.removeVariable",
            title: "Remove variable",
            arguments: [document, fixRange]
        }

        /*
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        let startPosition = new vscode.Position(currRange.start.line, currRange.start.character);
        let stopPosition = new vscode.Position(currRange.end.line + 1, 0)
        fix.edit.replace(document.uri, new vscode.Range(startPosition, stopPosition), "");
        */
        
        fix.isPreferred = true;
        return fix;
    }

}