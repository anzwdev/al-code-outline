import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';

export class CodeCopCodeActions implements vscode.CodeActionProvider {


    protected _toolsExtensionContext: DevToolsExtensionContext;
    public static readonly providedCodeActionKinds = [
        vscode.CodeActionKind.SourceFixAll,
        vscode.CodeActionKind.QuickFix
    ];

    provideCodeActions(document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, token: vscode.CancellationToken): vscode.CodeAction[] {
        // for each diagnostic entry that has the matching `code`, create a code action command
        let Actions: vscode.CodeAction[] = [];

        Actions = Actions.concat(context.diagnostics
            .filter(diagnostic => diagnostic.code === "AA0008")
            .map((d, i, arr) => this.createAA0008Fix(document, d)));

        Actions = Actions.concat(context.diagnostics
            .filter(diagnostic => diagnostic.code === "AA0005")
            .map((d, i, arr) => this.createAA0005Fix(document, d)));

        Actions = Actions.concat(context.diagnostics
            .filter(diagnostic => diagnostic.code === "AA0137")
            .map((d, i, arr) => this.createAA0137Fix(document, d)));

        Actions = Actions.concat(context.diagnostics
            .filter(diagnostic => diagnostic.code === "AA0139")
            .map((d, i, arr) => this.createAA0139Fix(document, d)));

        return Actions
    }


    private createAA0008Fix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        const fix = new vscode.CodeAction(`Add parentheses`, vscode.CodeActionKind.SourceFixAll);
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        fix.edit.insert(document.uri, new vscode.Position(currRange.end.line, currRange.end.character), '()');

        fix.isPreferred = true;
        return fix;
    }

    private createAA0137Fix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        const fix = new vscode.CodeAction(`Remove Varaiable`, vscode.CodeActionKind.QuickFix);
        fix.edit = new vscode.WorkspaceEdit();
        fix.diagnostics = [diagnostic];
        let currRange = diagnostic.range;
        let startPosition = new vscode.Position(currRange.start.line, currRange.start.character);
        let stopPosition = new vscode.Position(currRange.end.line + 1, 0)
        fix.edit.replace(document.uri, new vscode.Range(startPosition, stopPosition), "");
        fix.isPreferred = true;
        return fix;
    }

    private createAA0139Fix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
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


    private createAA0005Fix(document: vscode.TextDocument, diagnostic: vscode.Diagnostic): vscode.CodeAction {
        const fix = new vscode.CodeAction(`Remove begin..end`, vscode.CodeActionKind.QuickFix);
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