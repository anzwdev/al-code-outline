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
            //AA0005 fix disabled, needs some fixes before going live
            //new ALCodeCopFixAA0005(),
            new ALCodeCopFixAA0008(),
            new ALCodeCopFixAA0137(),
            new ALCodeCopFixAA0139()
        ]
    }

    provideCodeActions(document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, token: vscode.CancellationToken): vscode.CodeAction[] {
        // for each diagnostic entry that has the matching `code`, create a code action command
        let actions: vscode.CodeAction[] = [];

        for (let idx=0; idx<this.codeFixes.length; idx++) {
            actions = actions.concat(context.diagnostics
                .filter(diagnostic => diagnostic.code === this.codeFixes[idx].diagnosticCode)
                .map((d, i, arr) => this.codeFixes[idx].createFix(document, d)));
        }

        return actions
    }


}