import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsWorkspaceCommandRequest } from '../langserver/toolsWorkspaceCommandRequest';
import { TextRange } from '../symbollibraries/textRange';
import { NumberHelper } from '../tools/numberHelper';
import { TextEditorHelper } from '../tools/textEditorHelper';

export class SyntaxModifier {
    protected _commandName: string;
    protected _context: DevToolsExtensionContext;

    constructor(context: DevToolsExtensionContext, commandName: string) {
        this._commandName = commandName;
        this._context = context;
    }

    async RunForWorkspace() {
        let confirmation = await this.confirmRunForWorkspace();
        if (!confirmation)
            return;

        let workspaceUri = TextEditorHelper.getActiveWorkspaceFolderUri();
        vscode.workspace.saveAll();
        if (!workspaceUri)
            return;

        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest(this._commandName, '', workspaceUri.fsPath, undefined, this.getParameters(workspaceUri));
        let response = await this._context.toolsLangServerClient.workspaceCommand(request);

        if (response) {
            if ((response.error) && (response.errorMessage))
                vscode.window.showErrorMessage(response.errorMessage);
            else
                vscode.window.showInformationMessage(
                    NumberHelper.zeroIfNotDef(response.parameters.noOfChangedFiles).toString() +
                    ' file(s) modified.');
        }
    }

    protected async confirmRunForWorkspace(): Promise<boolean> {
        let confirmation = await vscode.window.showInformationMessage(
            'Do you want to run this command for all files in the current project folder?', 
            'Yes', 'No');
        return (confirmation === 'Yes');
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
        
        let request: ToolsWorkspaceCommandRequest = new ToolsWorkspaceCommandRequest(this._commandName, text, '', range, this.getParameters(document.uri));
        let response = await this._context.toolsLangServerClient.workspaceCommand(request);
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
                    vscode.window.showInformationMessage(
                        'Command completed.');
            } else if (withUI)
                vscode.window.showInformationMessage('There was nothing to change.');
        }
    }

}