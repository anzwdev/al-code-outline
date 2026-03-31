import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { DevToolsExtensionService } from "../devToolsExtensionService";
import { CodeOutlineTreeProvider } from "./codeOutlineTreeProvider";
import { CodeOutlineTreeItem } from './codeOutlineTreeItem';
import { CodeOutlineViewConst } from './codeOutlineViewConst';
import { CodeOutlineSortMode } from './codeOutlineSortMode';
import { CodeOutlineTreeLoader } from './codeOutlineTreeLoader';

export class CodeOutlineViewService extends DevToolsExtensionService {

    private _treeProvider: CodeOutlineTreeProvider;
    private _treeView: vscode.TreeView<CodeOutlineTreeItem>;
    private _followCursor: boolean = false;
    private _selectionChange: boolean;
    private _selectionChangedHandler: vscode.Disposable | undefined;

    constructor(context: DevToolsExtensionContext) {
        super(context);

        //get settings
        this._selectionChange = false;
        this._selectionChangedHandler = undefined;
        this._followCursor = !!this.context.vscodeExtensionContext.globalState.get<boolean>(CodeOutlineViewConst.cfgFollowCursor);

        //register symbols tree provider
        this._treeProvider = new CodeOutlineTreeProvider(this.context);
        this.subscriptions.push(
            vscode.window.registerTreeDataProvider(CodeOutlineViewConst.txtCodeOutlineProviderName, this._treeProvider));

        this._treeView = vscode.window.createTreeView<CodeOutlineTreeItem>(CodeOutlineViewConst.txtCodeOutlineProviderName, { 
            treeDataProvider: this._treeProvider
        });

        this.registerEvents();
        this.registerCommands();
    }

    private registerEvents() {
        this.subscriptions.push(
            this._treeView.onDidExpandElement(e => {
                this._treeProvider.setNodeState(e.element, vscode.TreeItemCollapsibleState.Expanded);
            }));

        this.subscriptions.push(            
            this._treeView.onDidCollapseElement(e => {
                this._treeProvider.setNodeState(e.element, vscode.TreeItemCollapsibleState.Collapsed);
            }));
    }

    private registerCommands() {
        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdEnableFollowCursor,
                () => this.setFollowCursor(true)));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdDisableFollowCursor,
                () => this.setFollowCursor(false)));
        
        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdRefreshOutlineView,
                () => this._treeProvider.refresh()));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdCollapseAll,
                () => this._treeProvider.collapseAll()));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSelectOutlineNodeText,
                (range: vscode.Range) => {
                    if ((!this._selectionChange) && (vscode.window.activeTextEditor)) {
                        vscode.window.activeTextEditor.revealRange(range, vscode.TextEditorRevealType.Default);
                        vscode.window.activeTextEditor.selection = new vscode.Selection(range.start, range.end);
                        vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');            
                    }
            }));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSortByPosition,
                () => {
                    this._treeProvider.setSortMode(CodeOutlineSortMode.position);
                }));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSortByPositionNoAction, () => {}));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSortByName,
                () => {
                    this._treeProvider.setSortMode(CodeOutlineSortMode.name);
                }));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSortByNameNoAction, () => { }));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSortByCategory,
                () => {
                    this._treeProvider.setSortMode(CodeOutlineSortMode.category);
                }));

        this.subscriptions.push(
            vscode.commands.registerCommand(
                CodeOutlineViewConst.cmdSortByCategoryNoAction, () => { }));
    }

    setFollowCursor(value: boolean) {
        this._followCursor = value;
        if ((this._followCursor) && (!this._selectionChangedHandler)) {
            this._selectionChangedHandler = vscode.window.onDidChangeTextEditorSelection((e) => {
                this.onTextEditorSelectionChanged(e);
            });
        } else if ((!this._followCursor) && (this._selectionChangedHandler)) {
            this._selectionChangedHandler.dispose();
            this._selectionChangedHandler = undefined;
        }

        vscode.commands.executeCommand('setContext', CodeOutlineViewConst.ctxFollowCursor, this._followCursor);
        this.context.vscodeExtensionContext.globalState.update(CodeOutlineViewConst.cfgFollowCursor, this._followCursor);
    }

    private async onTextEditorSelectionChanged(e: vscode.TextEditorSelectionChangeEvent) {
        if ((this._treeView.visible) && (e.selections.length > 0)) {
            let symbol = this._treeProvider.getNodeAtPosition(e.selections[0].active);
            if (symbol) {
                this._selectionChange = true;
                await this._treeView.reveal(symbol, {
                    select: true,
                    focus: false,
                    expand: false
                });
                this._selectionChange = false;
            }
        }
    }

    updateCurrentTreeLoader(treeLoader: CodeOutlineTreeLoader) {
        this._treeProvider.setTreeLoader(treeLoader);
    }


}