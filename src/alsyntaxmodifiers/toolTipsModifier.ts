import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { TextEditorHelper } from '../tools/textEditorHelper';
import { NumberHelper } from '../tools/numberHelper';
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { StringHelper } from '../tools/stringHelper';
import { SyntaxModifier } from './syntaxModifier';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';

export class ToolTipModifier extends SyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "addToolTips");
        this._showProgress = true;
        this._progressMessage = "Processing project files. Please wait...";
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);
        parameters.toolTipField = this.getFieldTooltip(uri),
        parameters.toolTipFieldComment = this.getFieldTooltipComment(uri),
        parameters.toolTipAction = this.getActionTooltip(uri),
        parameters.useFieldDescription = this.getUseFieldDescription(uri)
        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing tool tips to all files in the current project folder?',
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected showWorkspaceSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +
            ' toolTip(s) added to ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).');
    }

    protected showDocumentSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            response.parameters.noOfChanges.toString() +
            ' ToolTip(s) added.');
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

    protected getFieldTooltipComment(uri: vscode.Uri | undefined): string {
        let toolTip = StringHelper.emptyIfNotDef(
            vscode.workspace.getConfiguration('alOutline', uri).get('pageFieldToolTipComment'));
        if (toolTip == '') {
            toolTip = '%Caption.Comment%';
        }
        return toolTip;
    }

    protected getUseFieldDescription(uri: vscode.Uri | undefined): boolean {
        let value : boolean = !!vscode.workspace.getConfiguration('alOutline', uri).get<boolean>('useTableFieldDescriptionAsToolTip');
        return value;
    }

}
