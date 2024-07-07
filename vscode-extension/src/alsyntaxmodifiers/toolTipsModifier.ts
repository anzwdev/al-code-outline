import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { NumberHelper } from '../tools/numberHelper';
import { StringHelper } from '../tools/stringHelper';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';
import { WorkspaceCommandSyntaxModifier } from './workspaceCommandSyntaxModifier';
import { ALFieldToolTipsLocation } from '../allanguage/alFieldToolTipsLocation';

export class ToolTipModifier extends WorkspaceCommandSyntaxModifier {

    constructor(context: DevToolsExtensionContext) {
        super(context, "Add ToolTips", "addToolTips");
    }

    protected getParameters(uri: vscode.Uri): any {
        let parameters = super.getParameters(uri);

        let config = vscode.workspace.getConfiguration('alOutline', uri);
        
        parameters.toolTipField = this.getFieldTooltip(config);
        parameters.toolTipFieldComment = this.getFieldTooltipComment(config);
        parameters.toolTipAction = this.getActionTooltip(config);
        parameters.useFieldDescription = this.getUseFieldDescription(config);
        parameters.sortProperties = this.sortPropertiesOnSave(uri);
        parameters.createFieldToolTips = this.createFieldToolTips(uri);
        parameters.createActionToolTips = true;

        parameters.reuseToolTips = !config.get<boolean>('doNotReuseToolTipsFromOtherPages');
        let toolTipsSource = config.get<string[]>('reuseToolTipsFromDependencies');
        if (toolTipsSource) {
            for (let i=0; i<toolTipsSource.length; i++) {
                let name = "toolTipDependency" + i.toString();
                parameters[name] = toolTipsSource[i];
            }
        }

        return parameters;
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to add missing tool tips to all files in the current project folder?',
            'Yes', 'No');
        return (confirmation === 'Yes');
    }

    protected getSuccessWorkspaceMessage(response: ToolsWorkspaceCommandResponse): string {
        return NumberHelper.zeroIfNotDef(response.parameters.noOfChanges).toString() +
            ' toolTip(s) added to ' +
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s).';
    }

    protected getSuccessDocumentMessage(response: ToolsWorkspaceCommandResponse): string {
        return response.parameters.noOfChanges.toString() +
            ' ToolTip(s) added.';
    }

    protected getActionTooltip(config: vscode.WorkspaceConfiguration): string {
        let toolTip = StringHelper.emptyIfNotDef(
            config.get('pageActionToolTip'));
        if (toolTip == '') {
            toolTip = 'Executes the action %1';
        }
        return toolTip;
    }

    protected getFieldTooltip(config: vscode.WorkspaceConfiguration): string {
        let toolTip = StringHelper.emptyIfNotDef(
            config.get('pageFieldToolTip'));
        if (toolTip == '') {
            toolTip = 'Specifies the value for the field %1';
        }
        return toolTip;
    }

    protected getFieldTooltipComment(config: vscode.WorkspaceConfiguration): string {
        let toolTip = StringHelper.emptyIfNotDef(
            config.get('pageFieldToolTipComment'));
        if (toolTip === '') {
            toolTip = '%Caption.Comment%';
        }
        if (toolTip === '%%Caption.Comment%') {
            toolTip = '%Caption.Comment%';
        }
        return toolTip;
    }

    protected getUseFieldDescription(config: vscode.WorkspaceConfiguration): boolean {
        let value : boolean = !!config.get<boolean>('useTableFieldDescriptionAsToolTip');
        return value;
    }

    protected createFieldToolTips(uri: vscode.Uri | undefined): boolean {
        let location = this._context.alLangProxy.fieldToolTipsLocation(uri);
        return (location === ALFieldToolTipsLocation.page);
    }

}
