import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';
import { TextRange } from '../symbollibraries/textRange';
import { NumberHelper } from '../tools/numberHelper';
import { ISyntaxModifierResult } from './iSntaxModifierResult';
import { SyntaxModifier } from "./syntaxModifier";

export class WorkspaceCommandSyntaxModifier extends SyntaxModifier {
    protected _commandName: string;

    constructor(context: DevToolsExtensionContext, newName: string, commandName: string) {
        super(context, newName);
        this._commandName = commandName;
    }

    async runForWorkspaceWithoutUI(workspaceUri: vscode.Uri): Promise<ISyntaxModifierResult | undefined> {
        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest(this._commandName, '', workspaceUri.fsPath, undefined, undefined, this.getParameters(workspaceUri));       
        let response = await this.runWorkspaceCommand(request);

        if (response) {
            if ((response.error) && (response.errorMessage)) {
                let errorMessage = response.errorMessage;
                if (response.parameters)
                    errorMessage = NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() + " file(s) modified. " + errorMessage;
                return {
                    success: false,
                    message: errorMessage,
                    source: undefined
                };
            } else
                return {
                    success: true,
                    message: this.getSuccessWorkspaceMessage(response),
                    source: undefined
                }
        }
        return undefined;
    }

    async RunForDocumentWithoutUI(text: string, workspaceUri: vscode.Uri, documentUri: vscode.Uri, range: TextRange | undefined): Promise<ISyntaxModifierResult | undefined> {
        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest(this._commandName, text, workspaceUri.fsPath, documentUri.fsPath, range, this.getParameters(documentUri));
        let response = await this.runWorkspaceCommand(request);

        if (response) {

            if (response.error)
                return {
                    success: false,
                    message: response.errorMessage,
                    source: undefined
                };

            if ((response.source) && (response.source != text))
                return {
                    success: true,
                    message: this.getSuccessDocumentMessage(response),
                    source: response.source
                };
        }

        return {
            success: true,
            message: 'There was nothing to change.',
            source: undefined
        };
    }

    protected async runWorkspaceCommand(request: ToolsWorkspaceCommandRequest): Promise<ToolsWorkspaceCommandResponse | undefined> {
        if (this._showProgress)
            return await vscode.window.withProgress<ToolsWorkspaceCommandResponse | undefined>({
                    location: vscode.ProgressLocation.Notification,
                    title: this._progressMessage
                }, async (progress) => {
                    return await this._context.toolsLangServerClient.workspaceCommand(request);
                });
        return await this._context.toolsLangServerClient.workspaceCommand(request);
    }

    protected getSuccessWorkspaceMessage(response: ToolsWorkspaceCommandResponse): string {
        return NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s) modified.';
    }

    protected getSuccessDocumentMessage(response: ToolsWorkspaceCommandResponse): string {
        return 'Command completed';        
    }

}