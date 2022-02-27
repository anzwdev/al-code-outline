import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DuplicateCodeTreeNode } from '../duplicatecode/duplicateCodeTreeNode';
import { DuplicateCodeTreeProvider } from "../duplicatecode/duplicateCodeTreeProvider";
import { ToolsFindDuplicateCodeRequest } from '../langserver/toolsFindDuplicateCodeRequest';
import { DocumentTextRange } from '../symbollibraries/documentTextRange';
import { DevToolsExtensionService } from "./devToolsExtensionService";

export class DuplicateCodeService extends DevToolsExtensionService {
    protected _treeProvider: DuplicateCodeTreeProvider;
    protected _treeView: vscode.TreeView<DuplicateCodeTreeNode>;

    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        super(newContext);
        this._treeProvider = new DuplicateCodeTreeProvider();
       
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerTreeDataProvider('azALDevTools.DuplicateCodeTreeProvider', this._treeProvider));
        this._treeView = vscode.window.createTreeView<DuplicateCodeTreeNode>('azALDevTools.DuplicateCodeTreeProvider', { 
            treeDataProvider: this._treeProvider
        });

        //register commands
        this.registerCommands();
    }

    protected registerCommands() {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.findCodeDuplicates', 
                () => this.findDuplicates()));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showDuplicateCode', 
                (documentRange) => this.showDuplicateCode(documentRange)));
    }

    protected async findDuplicates() {
        let response = await this._context.toolsLangServerClient.findDuplicateCode(new ToolsFindDuplicateCodeRequest(3));
        if (!response)
            return;
        if (response.isError) {            
            vscode.window.showErrorMessage(response.message?response.message:'Unknown error');
            return;
        }
        if ((!response.duplicates) || (response.duplicates.length == 0)) {
            vscode.window.showInformationMessage('No duplicates found');
            return;
        }
        vscode.commands.executeCommand('setContext', 'azALDevTools.findDuplicateCodeActive', true);
        this._treeProvider.setDuplicates(response.duplicates);

        let firstDuplicate = this._treeProvider.getFirstDuplicateNode();
        if (firstDuplicate)
            this._treeView.reveal(firstDuplicate, {
                select: true,
                focus: true,
                expand: true
            });
    }

    protected async showDuplicateCode(documentRange: DocumentTextRange) {
        if (!documentRange.filePath)
            return;
        let vscodeRange = new vscode.Range(documentRange.start.line, documentRange.start.character, 
            documentRange.end.line, documentRange.end.character);

        let document = await vscode.workspace.openTextDocument(documentRange.filePath);
        let editor = await vscode.window.showTextDocument(document, {
            preview : true
        });
        editor.revealRange(vscodeRange, vscode.TextEditorRevealType.Default);
        editor.selection = new vscode.Selection(vscodeRange.start, vscodeRange.end);
        vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');
    }

    protected hideDuplicatedCode() {
        this._treeProvider.setDuplicates([]);
        vscode.commands.executeCommand('setContext', 'azALDevTools.findDuplicateCodeActive', false);
    }

}