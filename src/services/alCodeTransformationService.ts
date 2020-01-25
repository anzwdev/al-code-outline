import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsAddAppAreasRequest } from '../langserver/toolsAddAppAreasRequest';

export class ALCodeTransformationService {
    protected _context: DevToolsExtensionContext;
    
    constructor(context: DevToolsExtensionContext) {
        this._context = context;

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.AddApplicationAreas',
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

    async InsertApplicationArea(val: string) {
        if (!vscode.window.activeTextEditor)
            return;
        var text = vscode.window.activeTextEditor.document.getText();
        if (!text)
            return;
        
        let request: ToolsAddAppAreasRequest = new ToolsAddAppAreasRequest(text, '', val);
        let response = await this._context.toolsLangServerClient.addAppAreas(request);
        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else if ((response.source) && (response.source != text) && (response.noOfAppAreas > 0)) {
                text = response.source;
                const edit = new vscode.WorkspaceEdit();
                var firstLine = vscode.window.activeTextEditor.document.lineAt(0);
                var lastLine = vscode.window.activeTextEditor.document.lineAt(vscode.window.activeTextEditor.document.lineCount - 1);
                var textRange = new vscode.Range(0,
                    firstLine.range.start.character,
                    vscode.window.activeTextEditor.document.lineCount - 1,
                    lastLine.range.end.character);
                edit.replace(vscode.window.activeTextEditor.document.uri, textRange, text);
                vscode.workspace.applyEdit(edit);
                vscode.window.showInformationMessage(
                    response.noOfAppAreas.toString() + 
                    ' application area(s) added.');
            } else
                vscode.window.showInformationMessage('There are no missing application areas.');
        }
    }




}