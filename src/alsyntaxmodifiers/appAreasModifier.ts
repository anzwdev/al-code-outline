import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsAddAppAreasRequest } from '../langserver/toolsAddAppAreasRequest';
import { TextEditorHelper } from '../tools/textEditorHelper';
import { NumberHelper } from '../tools/numberHelper';

export class AppAreasModifier {
    private _context:DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async AddMissinAppAreaToWorkspace(name: string | undefined) {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing application areas to all files in the current project folder?', 
            'Yes', 'No');
        if (confirmation !== 'Yes')
            return;

        if (!name)
            name = await this.getApplicationAreaName();
        if (!name)
            return;

        let workspaceUri = TextEditorHelper.getActiveWorkspaceFolderUri();
        vscode.workspace.saveAll();
        if (!workspaceUri)
            return;
        
        let request: ToolsAddAppAreasRequest = new ToolsAddAppAreasRequest('', workspaceUri.fsPath, name);
        let response = await this._context.toolsLangServerClient.addAppAreas(request);
        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else
                vscode.window.showInformationMessage(
                    NumberHelper.zeroIfNotDef(response.noOfAppAreas).toString() +                    
                    ' application area(s) added to ' +
                    NumberHelper.zeroIfNotDef(response.noOfFiles).toString() +
                    ' file(s).');
        }
    }

    async AddMissingAppAreaToActiveEditor(name: string | undefined) {
        if (!name)
            name = await this.getApplicationAreaName();
        if (!name)
            return;
            
        if (!vscode.window.activeTextEditor)
            return;
        var text = vscode.window.activeTextEditor.document.getText();
        if (!text)
            return;
        
        let request: ToolsAddAppAreasRequest = new ToolsAddAppAreasRequest(text, '', name);
        let response = await this._context.toolsLangServerClient.addAppAreas(request);
        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else if ((response.source) && (response.source != text) && (response.noOfAppAreas) && (response.noOfAppAreas > 0)) {
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

    async getApplicationAreaName(): Promise<string | undefined> {
        let appAreasList = ['Basic', 'FixedAsset', 'All', 'Custom'];
        
        //ask for Application Area Type
        let appAreaName = await vscode.window.showQuickPick(appAreasList, {
            canPickMany: false,
            placeHolder: 'Select Application Area'
        });
        if (appAreaName === 'Custom')
            appAreaName = await vscode.window.showInputBox({
                placeHolder: "Enter your custom Application Area"
            });
        return appAreaName;
    }

}
