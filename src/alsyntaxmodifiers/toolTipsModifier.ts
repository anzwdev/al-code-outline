import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextEditorHelper } from '../tools/textEditorHelper';
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { StringHelper } from '../tools/stringHelper';

export class ToolTipModifier {
    private _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext) {
        this._context = context;
    }

    async AddMissinToolTipToWorkspace(name: string | undefined) {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing tool tips to all files in the current project folder?',
            'Yes', 'No');
        if (confirmation !== 'Yes')
            return;

        let workspaceUri = TextEditorHelper.getActiveWorkspaceFolderUri();
        vscode.workspace.saveAll();
        if (!workspaceUri)
            return;
        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest('addToolTips', '', workspaceUri.fsPath, {
            toolTipField: this.getFieldTooltip(workspaceUri), 
            toolTipAction: this.getActionTooltip(workspaceUri)
        });
        let response = await this._context.toolsLangServerClient.workspaceCommand(request);

        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else
                vscode.window.showInformationMessage(
                    NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +
                    ' toolTip(s) added to ' +
                    NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
                    ' file(s).');
        }
    }

    async AddMissingToolTipToActiveEditor(name: string | undefined) {
        if (!vscode.window.activeTextEditor)
            return;
        var text = vscode.window.activeTextEditor.document.getText();
        if (!text)
            return;
        let documentUri = vscode.window.activeTextEditor.document.uri;

        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest('addToolTips', text, '', {
            toolTipField: this.getFieldTooltip(documentUri), 
            toolTipAction: this.getActionTooltip(documentUri)
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
                    ' ToolTip(s) added.');
            } else
                vscode.window.showInformationMessage('There are no missing tool tips.');
        }
    }

    protected getActionTooltip(uri: vscode.Uri | undefined): string {
        let toolTip = StringHelper.emptyIfNotDef(
            vscode.workspace.getConfiguration('alOutline', uri).get('pageActionToolTip'));
        if (toolTip == '') {
            toolTip = 'Executes the action %1';
        }
        return toolTip;
    }

    protected getFieldTooltip(uri: vscode.Uri | undefined): string {
        let toolTip = StringHelper.emptyIfNotDef(
            vscode.workspace.getConfiguration('alOutline', uri).get('pageFieldToolTip'));
        if (toolTip == '') {
            toolTip = 'Specifies the value for the field %1';
        }
        return toolTip;
    }

}
