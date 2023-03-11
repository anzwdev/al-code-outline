import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SymbolsTreeProvider } from "../outlineview/symbolsTreeProvider";
import { ALSymbolsBasedPageWizard } from '../objectwizards/symbolwizards/alSymbolsBasedPageWizard';
import { ALSymbolsBasedReportWizard } from '../objectwizards/symbolwizards/alSymbolsBasedReportWizard';
import { ALSymbolsBasedXmlPortWizard } from '../objectwizards/symbolwizards/alSymbolsBasedXmlPortWizard';
import { ALSymbolsBasedQueryWizard } from '../objectwizards/symbolwizards/alSymbolsBasedQueryWizard';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { ALOutlineTreeItem } from '../outlineview/alOutlineTreeNode';
import { ALOutlineSortMode } from '../outlineview/alOutlineSortMode';

export class ALOutlineService {
    context: DevToolsExtensionContext;
    symbolsTreeProvider: SymbolsTreeProvider;
    treeView: vscode.TreeView<ALOutlineTreeItem>;
    protected _selectionChange: boolean;
    protected _selectionChangedHandler: vscode.Disposable | undefined;
    protected _followCursor: boolean;
    
    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        this.context = newContext;
        this._selectionChange = false;
        this._selectionChangedHandler = undefined;
        this._followCursor = !!this.context.vscodeExtensionContext.globalState.get<boolean>("azALDevTools.alOutlineFollowCursor");

        //register symbols tree provider
        this.symbolsTreeProvider = new SymbolsTreeProvider(this.context);
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerTreeDataProvider('azALDevTools.SymbolsTreeProvider', this.symbolsTreeProvider));
        this.treeView = vscode.window.createTreeView<ALOutlineTreeItem>('azALDevTools.SymbolsTreeProvider', { 
            treeDataProvider: this.symbolsTreeProvider
        });

        //register commands
        this.registerCommands();

        //initialize follow cursor functionality
        this.setFollowCursor(this._followCursor);
    }

    protected registerCommands() {
        this.context.vscodeExtensionContext.subscriptions.push(
            this.treeView.onDidCollapseElement((e) => {
                e.element.collapsibleState = vscode.TreeItemCollapsibleState.Collapsed;
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            this.treeView.onDidExpandElement((e) => {
                e.element.collapsibleState = vscode.TreeItemCollapsibleState.Expanded;
            }));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.alOutlineEnableFollowCursor', 
                () => this.setFollowCursor(true)));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.alOutlineDisableFollowCursor', 
                () => this.setFollowCursor(false)));
        
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.refreshOutlineView', 
                () => this.symbolsTreeProvider.refresh()));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.alOutlineCollapseAll', 
                () => this.symbolsTreeProvider.collapseAll()));
    
        //al symbols commands
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
            'alOutline.createCardPage', 
            offset => {
                let builder = new ALSymbolsBasedPageWizard(this.context);
                builder.showPageWizard(offset.symbol, 'Card');
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createListPage', 
                offset => {
                    let builder = new ALSymbolsBasedPageWizard(this.context);
                    builder.showPageWizard(offset.symbol, 'List');
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createReport', 
                offset => {
                    let builder = new ALSymbolsBasedReportWizard(this.context);
                    builder.showReportWizard(offset.symbol);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createXmlPort', 
                offset => {
                    let builder = new ALSymbolsBasedXmlPortWizard(this.context);
                    builder.showXmlPortWizard(offset.symbol);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createQuery', 
                offset => {
                    let builder = new ALSymbolsBasedQueryWizard(this.context);
                    builder.showQueryWizard(offset.symbol);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runPage', offset => {
                this.context.objectRunner.runSymbolAsync(offset.symbol);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runTable', 
                offset => {
                    this.context.objectRunner.runSymbolAsync(offset.symbol);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runReport', 
                offset => {
                    this.context.objectRunner.runSymbolAsync(offset.symbol);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.selectDocumentText',
                (range) => {
                    if ((!this._selectionChange) && (vscode.window.activeTextEditor)) {
                        let vscodeRange = new vscode.Range(range.start.line, range.start.character, 
                            range.end.line, range.end.character);
    
                        vscode.window.activeTextEditor.revealRange(vscodeRange, vscode.TextEditorRevealType.Default);
                        vscode.window.activeTextEditor.selection = new vscode.Selection(vscodeRange.start, vscodeRange.end);
                        vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');            
                    }
            }));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortOutlineViewByPosition',
                () => {
                    this.symbolsTreeProvider.setSortMode(ALOutlineSortMode.position)
                }));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortOutlineViewByPositionNoAction', () => {}));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
            'azALDevTools.sortOutlineViewByName',
            () => {
                this.symbolsTreeProvider.setSortMode(ALOutlineSortMode.name)
            }));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortOutlineViewByNameNoAction', () => { }));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortOutlineViewByCategory',
                () => {
                    this.symbolsTreeProvider.setSortMode(ALOutlineSortMode.category)
                }));

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.sortOutlineViewByCategoryNoAction', () => { }));

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

        vscode.commands.executeCommand('setContext', 'azALDevTools:alOutlineFollowCursor', this._followCursor);
        this.context.vscodeExtensionContext.globalState.update("azALDevTools.alOutlineFollowCursor", this._followCursor);
    }

    private async onTextEditorSelectionChanged(e: vscode.TextEditorSelectionChangeEvent) {
        if ((this.treeView.visible) && (e.selections.length > 0)) {
            let symbol = this.symbolsTreeProvider.getNodeAtPosition(e.selections[0].active);
            if (symbol) {
                this._selectionChange = true;
                await this.treeView.reveal(symbol, {
                    select: true,
                    focus: false,
                    expand: false
                });
                this._selectionChange = false;
            }
        }
    }


}