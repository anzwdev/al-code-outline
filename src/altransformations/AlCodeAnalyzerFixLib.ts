import * as vscode from 'vscode';
import { CodeCopCodeActions } from './alCodeCopCodeAction';

export class ALCodeAnalyzerFix {

    private context: vscode.ExtensionContext;
    constructor(_context: vscode.ExtensionContext) {
        this.context = _context;
    }

    async loadActions() {

        // Load Code Actions
        this.context.subscriptions.push(
            vscode.languages.registerCodeActionsProvider('al', new CodeCopCodeActions(), {
                providedCodeActionKinds: CodeCopCodeActions.providedCodeActionKinds
            })
        );

        // Others
        this.context.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.SpreadApplicationArea',
                () => {
                    let Areas = ["Basic", "FixedAsset", "All", "Custom"];

                    //ask for Application Area Type
                    vscode.window.showQuickPick(Areas, {
                        canPickMany: false,
                        placeHolder: 'Select Application Area'
                    }).then(val => {
                        if (val === "Custom") {
                            vscode.window.showInputBox({
                                placeHolder: "Enter your custom Application Area"
                            }).then(valCust => this.InsertApplicationArea(valCust));
                        } else {
                            this.InsertApplicationArea(val);
                        }
                    });
                }
            )
        );

    }

    InsertApplicationArea(val: string) {
        if (!vscode.window.activeTextEditor)
            return;
        var text = vscode.window.activeTextEditor.document.getText();
        if (!text)
            return;
        
        
        

    }


    /*
    InsertApplicationArea(val: string) {
        var re = /^([\w\-]+)/g;
        var text = vscode.window.activeTextEditor.document.getText();
        var ObjType = text.match(re);

        if ((ObjType[0].toLowerCase() == "page") || (ObjType[0].toLowerCase() == "pageextension") || (ObjType[0].toLowerCase() == "report")) {
            re = /((\bfield\(.*\)([\S\s]*?))})/g;
            var match = text.match(re);
            if (match != null) {
                match.forEach(m => {
                    re = /((applicationarea=all)|(applicationarea =all)|(applicationarea = all));/i
                    let appMatch = m.match(re);
                    let tempField = '';
                    if (appMatch == null) {
                        tempField = m.replace("{", "{\nApplicationArea = " + val + ";");
                        text = text.replace(m, tempField);
                    }
                });
            }

            re = /((\baction\(.*\)([\S\s]*?))})/g;
            var match = text.match(re);
            if (match != null) {
                match.forEach(m => {
                    re = /((applicationarea=all)|(applicationarea =all)|(applicationarea = all));/i
                    let appMatch = m.match(re);
                    let tempField = '';
                    if (appMatch == null) {
                        tempField = m.replace("{", "{\nApplicationArea = " + val + ";");
                        text = text.replace(m, tempField);
                    }
                });
            }

            const edit = new vscode.WorkspaceEdit();
            var firstLine = vscode.window.activeTextEditor.document.lineAt(0);
            var lastLine = vscode.window.activeTextEditor.document.lineAt(vscode.window.activeTextEditor.document.lineCount - 1);
            var textRange = new vscode.Range(0,
                firstLine.range.start.character,
                vscode.window.activeTextEditor.document.lineCount - 1,
                lastLine.range.end.character);
            edit.replace(vscode.window.activeTextEditor.document.uri, textRange, text);
            vscode.workspace.applyEdit(edit);
        }
    }
    */
}
