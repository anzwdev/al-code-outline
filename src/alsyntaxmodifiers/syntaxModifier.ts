import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { ToolsWorkspaceCommandResponse } from '../langserver/toolsWorkspaceCommandResponse';
import { TextRange } from '../symbollibraries/textRange';
import { NumberHelper } from '../tools/numberHelper';
import { TextEditorHelper } from '../tools/textEditorHelper';

export class SyntaxModifier {
    protected _commandName: string;
    protected _context: DevToolsExtensionContext;
    protected _showProgress: boolean;
    protected _progressMessage: string;

    constructor(context: DevToolsExtensionContext, commandName: string) {
        this._commandName = commandName;
        this._context = context;
        this._showProgress = false;
        this._progressMessage = 'Please wait...';
    }

    async RunForWorkspace() {
        let confirmation = await this.confirmRunForWorkspace();
        if (!confirmation)
            return;

        await this.askForParameters();

        let workspaceUri = TextEditorHelper.getActiveWorkspaceFolderUri();
        vscode.workspace.saveAll();
        if (!workspaceUri)
            return;

        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest(this._commandName, '', workspaceUri.fsPath, undefined, undefined, this.getParameters(workspaceUri));       
        let response = await this.runWorkspaceCommand(request);

        if (response) {
            if ((response.error) && (response.errorMessage)) {
                let errorMessage = response.errorMessage;
                if (response.parameters)
                errorMessage = NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() + " file(s) modified. " + errorMessage;
                vscode.window.showErrorMessage(errorMessage);
            } else
                this.showWorkspaceSuccessMessage(response);
        }
    }

    protected showWorkspaceSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
            ' file(s) modified.');
    }

    protected showDocumentSuccessMessage(response: ToolsWorkspaceCommandResponse) {
        vscode.window.showInformationMessage(
            'Command completed.');
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to run this command for all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
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

    protected getParameters(uri: vscode.Uri): any {
        return {};
    }

    async RunForActiveEditor() {
        if (!vscode.window.activeTextEditor)
            return;
        await this.RunForDocument(vscode.window.activeTextEditor.document, undefined, true);
    }

    async RunForDocument(document: vscode.TextDocument, range: TextRange | undefined, withUI: boolean) {
        let text = document.getText();

        if (!text)
            return;

        await this.askForParameters();

        let workspaceUri = TextEditorHelper.getActiveWorkspaceFolderUri();
        let workspacePath = '';
        if (workspaceUri)
            workspacePath = workspaceUri.fsPath;                
        let filePath: string | undefined = undefined;
        if (document.uri)
            filePath = document.uri.fsPath;

        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest(this._commandName, text, workspacePath, filePath, range, this.getParameters(document.uri));
        let response = await this.runWorkspaceCommand(request);
        if (response) {
            if ((response.error) && (response.errorMessage)) {
                if (withUI)
                    vscode.window.showErrorMessage(response.errorMessage);
            } else if ((response.source) && (response.source != text)) {
                text = response.source;
                const edit = new vscode.WorkspaceEdit();
                var firstLine = document.lineAt(0);
                var lastLine = document.lineAt(document.lineCount - 1);
                var textRange = new vscode.Range(0,
                    firstLine.range.start.character,
                    document.lineCount - 1,
                    lastLine.range.end.character);
                edit.replace(document.uri, textRange, text);
                await vscode.workspace.applyEdit(edit);
                if (withUI)
                    this.showDocumentSuccessMessage(response);
            } else if (withUI)
                vscode.window.showInformationMessage('There was nothing to change.');
        }
    }

    protected async askForParameters() {
    }

}