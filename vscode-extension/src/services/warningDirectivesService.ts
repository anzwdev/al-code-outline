import * as vscode from 'vscode';
import { WarningDirectivesTreeNode } from "../codeanalyzers/warningDirectivesTreeNode";
import { WarningDirectivesTreeProvider } from "../codeanalyzers/warningDirectivesTreeProvider";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { ToolsGetWarningDirectivesRequest } from '../langserver/symbolsinformation/toolsGetWarningDirectivesRequest';
import { WarningDirectiveInfoKind } from '../symbolsinformation/warningDirectiveInfoKind';
import { DevToolsExtensionService } from "./devToolsExtensionService";

export class WarningDirectivesService extends DevToolsExtensionService {
    protected _treeProvider: WarningDirectivesTreeProvider;
    protected _treeView: vscode.TreeView<WarningDirectivesTreeNode>;

    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        super(newContext);
        this._treeProvider = new WarningDirectivesTreeProvider(newContext);
       
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerTreeDataProvider('azALDevTools.WarningDirectivesTreeView', this._treeProvider));

        this._treeView = vscode.window.createTreeView<WarningDirectivesTreeNode>('azALDevTools.WarningDirectivesTreeView', { 
            treeDataProvider: this._treeProvider
        });

        //register commands
        this.registerCommands();
    }

    private registerCommands() {
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showWarningDirectives',
                () => this.showWarningDirectives()));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showWarningDirectiveItem', 
                (node) => this.showWarningDirectiveItem(node)));
        
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.refreshWarningDirectivesPanel',
                () => this.showWarningDirectives()));
        
        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.hideWarningDirectivesPanel',
                () => this.hideWarningDirectives()));
    }

    private async showWarningDirectives() {
        var request = new ToolsGetWarningDirectivesRequest(undefined);
        let response = await this._context.toolsLangServerClient.getWarningDirectives(request);
        vscode.commands.executeCommand('setContext', 'azALDevTools.warningDirectivesActive', true);
        if (response)
            this._treeProvider.setWarningDirectives(response.directives);

        let firstNode = this._treeProvider.getFirstNode();
        if (firstNode)
            this._treeView.reveal(firstNode, {
                select: true,
                focus: true,
                expand: true
            });
    
    }

    protected hideWarningDirectives() {
        this._treeProvider.setWarningDirectives(undefined);
        vscode.commands.executeCommand('setContext', 'azALDevTools.warningDirectivesActive', false);
    }

    private async showWarningDirectiveItem(node: WarningDirectivesTreeNode) {
        let fsPath: string | undefined = undefined;
        let range: vscode.Range | undefined = undefined;

        if (node.directiveInfo) {
            switch (node.directiveInfo.kind) {
                case WarningDirectiveInfoKind.File:
                    fsPath = node.directiveInfo.fullPath;
                    break;
                case WarningDirectiveInfoKind.DirectiveLocation:
                    if ((node.parent) && (node.parent.directiveInfo))
                        fsPath = node.parent.directiveInfo.fullPath;
                    if (node.directiveInfo.range)
                        range = new vscode.Range(node.directiveInfo.range.start.line, node.directiveInfo.range.start.character, 
                            node.directiveInfo.range.end.line, node.directiveInfo.range.end.character);
                break;
            }
        }

        if (fsPath) {
            let document = await vscode.workspace.openTextDocument(fsPath);
            let editor = await vscode.window.showTextDocument(document, {
                preview : true
            });

            if (range) {
                editor.revealRange(range, vscode.TextEditorRevealType.Default);
                editor.selection = new vscode.Selection(range.start, range.end);
            }
            //vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');
        }
    }

}