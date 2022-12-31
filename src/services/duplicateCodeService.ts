import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { DuplicateCodeSortMode } from '../duplicatecode/duplicateCodeSortMode';
import { DuplicateCodeTreeNode } from '../duplicatecode/duplicateCodeTreeNode';
import { DuplicateCodeTreeProvider } from "../duplicatecode/duplicateCodeTreeProvider";
import { ToolsFindDuplicateCodeRequest } from '../langserver/toolsFindDuplicateCodeRequest';
import { ALObsoleteState } from '../symbollibraries/alObsoleteState';
import { DocumentTextRange } from '../symbollibraries/documentTextRange';
import { QuickPickHelper } from '../tools/quickPickHelper';
import { TypedQuickPickItem } from '../tools/typedQuickPickItem';
import { DevToolsExtensionService } from "./devToolsExtensionService";

export class DuplicateCodeService extends DevToolsExtensionService {
    protected _treeProvider: DuplicateCodeTreeProvider;
    protected _treeView: vscode.TreeView<DuplicateCodeTreeNode>;
    protected _minAllowedNoOfStatements: number;

    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        super(newContext);
        this._minAllowedNoOfStatements = 3;
        this._treeProvider = new DuplicateCodeTreeProvider(newContext);
       
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
                'azALDevTools.findDuplicateCode', 
                () => this.findDuplicates()));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showDuplicateCode', 
                (documentRange) => this.showDuplicateCode(documentRange)));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.refreshDuplicateCodePanel',
                () => this.findDuplicates()));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.hideDuplicateCodePanel',
                () => this.hideDuplicateCodePanel()));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortDuplicatesByNoOfStatements',
                () => this.sortBy(DuplicateCodeSortMode.noOfStatements)));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortDuplicatesByType',
                () => this.sortBy(DuplicateCodeSortMode.codeBlockType)));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortDuplicatesByNoOfDuplicates',
                () => this.sortBy(DuplicateCodeSortMode.noOfDuplicates)));

        this._context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortDuplicatesByTypeNoOfDuplicates',
                () => this.sortBy(DuplicateCodeSortMode.codeBlockTypeNoOfDuplicates)));
                        
    }

    protected sortBy(sortMode: DuplicateCodeSortMode) {
        this._treeProvider.sortDuplicates(sortMode, false);
    }

    protected async findDuplicates() {
        let selectedFolder = await QuickPickHelper.pickWorkspaceFolder(true);
        if (!selectedFolder)
            return;
        let duplicatesPath : string | undefined = (selectedFolder.folder) ? selectedFolder.folder.uri.fsPath : undefined;

        let minNoOfStatements = await this.getMinNoOfStatements();
        if (minNoOfStatements <= 0)
            return;

        let skipObsoleteCodeLevel = await this.getObsoleteStateLevel();
        if (skipObsoleteCodeLevel === undefined)
            return;

        let response = await this._context.toolsLangServerClient.findDuplicateCode(new ToolsFindDuplicateCodeRequest(minNoOfStatements, skipObsoleteCodeLevel, duplicatesPath));
        if (!response)
            return;
        if (response.isError) {            
            vscode.window.showErrorMessage(response.message?response.message:'Unknown error');
            return;
        }
        if ((!response.duplicates) || (response.duplicates.length == 0)) {
            this._treeProvider.setDuplicates([]);
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

    protected hideDuplicateCodePanel() {
        this._treeProvider.setDuplicates([]);
        vscode.commands.executeCommand('setContext', 'azALDevTools.findDuplicateCodeActive', false);
    }

    protected async getMinNoOfStatements(): Promise<number> {
        let defaultValue = this.getDefaultMinNoOfStatements();
        let valueString = await vscode.window.showInputBox({
            prompt: "Min. no of statements",
            value: defaultValue.toString(),
            validateInput: (text: string): string | undefined => {
                if (text) {
                    let value = Number.parseInt(text);
                    if ((value) && (!Number.isNaN(value)) && (value >= this._minAllowedNoOfStatements))
                        return undefined;       
                }
                return 'Min. no of statements must be a number greate or equal 3';
            }
        });

        if (valueString) {
            let value = Number.parseInt(valueString);
            if ((value) && (!Number.isNaN(value)) && (value >= this._minAllowedNoOfStatements)) {
                this.setDefaultMinNoOfStatements(value);
                return value;
            }
        }

        return 0;
    }

    protected async getObsoleteStateLevel() : Promise<ALObsoleteState | undefined> {
        let defaultValue = this.getDefaultObsoleteState();
        let obsoleteStatesList = [
            new TypedQuickPickItem<ALObsoleteState>('None', ALObsoleteState.None, defaultValue == ALObsoleteState.None),
            new TypedQuickPickItem<ALObsoleteState>('Pending', ALObsoleteState.Pending, defaultValue == ALObsoleteState.Pending),
            new TypedQuickPickItem<ALObsoleteState>('Removed', ALObsoleteState.Removed, defaultValue == ALObsoleteState.Removed)];
        
        //ask for obsolete state level
        let obsoleteState = await vscode.window.showQuickPick(obsoleteStatesList, {
            canPickMany: false,                        
            placeHolder: 'Select obsolete state level to ignore'
        });

        if (!obsoleteState)
            return undefined;

        this.setDefaultObsoleteState(obsoleteState.value);

        return obsoleteState.value;
    }

    protected getDefaultMinNoOfStatements() : number {
        let value = this._context.vscodeExtensionContext.globalState.get<number>("azALDevTools.duplCode.minNoOfStatements");
        if ((value) && (value >= this._minAllowedNoOfStatements))
            return value;
        return this._minAllowedNoOfStatements;
    }

    protected setDefaultMinNoOfStatements(value: number) {
        this._context.vscodeExtensionContext.globalState.update("azALDevTools.duplCode.minNoOfStatements", value);
    }

    protected getDefaultObsoleteState() : ALObsoleteState {
        let value = this._context.vscodeExtensionContext.globalState.get<ALObsoleteState>("azALDevTools.duplCode.obsoleteState");
        if (value === undefined)
            return ALObsoleteState.None;
        return value;        
    }

    protected setDefaultObsoleteState(value: ALObsoleteState) {
        this._context.vscodeExtensionContext.globalState.update("azALDevTools.duplCode.obsoleteState", value);
    }


}