import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextEditorHelper } from '../tools/textEditorHelper';
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';

export class DataClassificationModifier {
    private _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async AddMissinDataClassificationToWorkspace() {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing data classification to all files in the current project folder?', 
            'Yes', 'No');
        if (confirmation !== 'Yes')
            return;

        let name: string | undefined = await this.getDataClassification();
        if (!name)
            return;

        let workspaceUri = TextEditorHelper.getActiveWorkspaceFolderUri();
        vscode.workspace.saveAll();
        if (!workspaceUri)
            return;

        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest('addDataClassification', '', workspaceUri.fsPath, {
            dataClassification: name
        });
        let response = await this._context.toolsLangServerClient.workspaceCommand(request);

        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else
                vscode.window.showInformationMessage(
                    NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +                    
                    ' data classification(s) added to ' +
                    NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
                    ' file(s).');
        }
    }

    async AddMissingDataClassificationToActiveEditor() {
        let name: string | undefined = await this.getDataClassification();
        if (!name)
            return;
            
        if (!vscode.window.activeTextEditor)
            return;
        var text = vscode.window.activeTextEditor.document.getText();
        if (!text)
            return;
        
        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest('addDataClassification', text, '', {
            dataClassification: name
        });
        let response = await this._context.toolsLangServerClient.workspaceCommand(request);
        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else if ((response.source) && (response.source != text) && (response.parameters.noOfChanges) && (response.parameters.noOfChanges > 0)) {
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
                    response.parameters.noOfChanges.toString() + 
                    ' data classification(s) added.');
            } else
                vscode.window.showInformationMessage('There are no missing data classifications.');
        }
    }

    async getDataClassification(): Promise<string | undefined> {
        let valuesList = ['AccountData', 'CustomerContent',
            'EndUserIdentifiableInformation', 'EndUserPseudonymousIdentifiers',
            'OrganizationIdentifiableInformation', 'SystemMetadata'];

        //ask for Application Area Type
        let value = await vscode.window.showQuickPick(valuesList, {
            canPickMany: false,
            placeHolder: 'Select Data Classification'
        });
        return value;
    }

}