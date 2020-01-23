import * as vscode from 'vscode';
import { ALCodeFix } from './alCodeFix';
import { ALCodeCopFixAA0005 } from './alCodeCopFIxAA0005';
import { ALCodeCopFixAA0008 } from './alCodeCopFixAA0008';
import { ALCodeCopFixAA0137 } from './alCodeCopFixAA0137';
import { ALCodeCopFixAA0139 } from './alCodeCopFixAA0139';

export class ALCAFixesCodeActionsProvider implements vscode.CodeActionProvider {   
    public static readonly providedCodeActionKinds = [
        vscode.CodeActionKind.SourceFixAll,
        vscode.CodeActionKind.QuickFix
    ];

    protected codeFixes: ALCodeFix[];

    constructor() {
        this.codeFixes = [
            new ALCodeCopFixAA0005(),
            new ALCodeCopFixAA0008(),
            new ALCodeCopFixAA0137(),
            new ALCodeCopFixAA0139()
        ]
    }

    provideCodeActions(document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, token: vscode.CancellationToken): vscode.CodeAction[] {
        // for each diagnostic entry that has the matching `code`, create a code action command
        let Actions: vscode.CodeAction[] = [];

        for (let i=0; i<this.codeFixes.length; i++) {
            Actions = Actions.concat(context.diagnostics
                .filter(diagnostic => diagnostic.code === this.codeFixes[i].diagnosticCode)
                .map((d, i, arr) => this.codeFixes[i].createFix(document, d)));
        }

        return Actions
    }


}