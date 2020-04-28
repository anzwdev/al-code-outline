import * as vscode from 'vscode';
import { ALCodeFix } from "../alCodeFix";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALCodeCopFixAA0139 extends ALCodeFix {

    constructor(context : DevToolsExtensionContext) {
        super(context, "AA0139");
    }

    createFix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction | undefined {
        const fix = new vscode.CodeAction(`Add CopyStr`, vscode.CodeActionKind.QuickFix);
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        let leftSideRange = new vscode.Range(currRange.start.line, 0, currRange.start.line, currRange.start.character);
        let leftSideText = document.getText(leftSideRange).trim();
        if (leftSideText.endsWith(':=')) {
            leftSideText = leftSideText.substr(0, leftSideText.length - 2).trim();
            if (leftSideText) {
                let stmt = "CopyStr(" + document.getText(currRange) + ", 1, MaxStrLen(" + leftSideText + "))";
                fix.edit.replace(document.uri, currRange, stmt);
                fix.isPreferred = true;
                return fix;
            }
        }
        
       return undefined;
    }

}