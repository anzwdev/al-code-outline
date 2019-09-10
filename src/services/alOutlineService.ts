import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { SymbolsTreeProvider } from "../outlineview/symbolsTreeProvider";
import { ALSymbolsBasedPageWizard } from '../objectwizards/symbolwizards/alSymbolsBasedPageWizard';
import { ALSymbolsBasedReportWizard } from '../objectwizards/symbolwizards/alSymbolsBasedReportWizard';
import { ALSymbolsBasedXmlPortWizard } from '../objectwizards/symbolwizards/alSymbolsBasedXmlPortWizard';
import { ALSymbolsBasedQueryWizard } from '../objectwizards/symbolwizards/alSymbolsBasedQueryWizard';
import { SymbolsTreeView } from '../symbolstreeview/symbolsTreeView';

export class ALOutlineService {
    context: DevToolsExtensionContext;
    symbolsTreeProvider: SymbolsTreeProvider;
    
    constructor(newContext: DevToolsExtensionContext) {
        //initialize
        this.context = newContext;

        //register symbols tree provider
        this.symbolsTreeProvider = new SymbolsTreeProvider(this.context);
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.window.registerTreeDataProvider('azALDevTools_SymbolsTreeProvider', this.symbolsTreeProvider));
    
        //register commands
        this.registerCommands();
    }

    protected registerCommands() {
        let that = this;

        //outline preview window
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.showDocumentSymbols',
                () => that.showSymbolsTreeView()
            )
        );

        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.refreshOutlineView', 
                () => that.symbolsTreeProvider.refresh()));
    
        //al symbols commands
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
            'alOutline.createCardPage', 
            offset => {
                let builder = new ALSymbolsBasedPageWizard();
                builder.showPageWizard(offset, 'Card');
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createListPage', 
                offset => {
                    let builder = new ALSymbolsBasedPageWizard();
                    builder.showPageWizard(offset, 'List');
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createReport', 
                offset => {
                    let builder = new ALSymbolsBasedReportWizard();
                    builder.showReportWizard(offset);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createXmlPort', 
                offset => {
                    let builder = new ALSymbolsBasedXmlPortWizard();
                    builder.showXmlPortWizard(offset);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.createQuery', 
                offset => {
                    let builder = new ALSymbolsBasedQueryWizard();
                    builder.showQueryWizard(offset);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runPage', offset => {
                that.context.objectRunner.runSymbolAsync(offset);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runTable', 
                offset => {
                    that.context.objectRunner.runSymbolAsync(offset);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'alOutline.runReport', 
                offset => {
                    that.context.objectRunner.runSymbolAsync(offset);
            }));
        this.context.vscodeExtensionContext.subscriptions.push(
            vscode.commands.registerCommand(
                'azALDevTools.selectDocumentText',
                (range) => {
                    if (vscode.window.activeTextEditor) {
                        let vscodeRange = new vscode.Range(range.start.line, range.start.character, 
                            range.end.line, range.end.character);
    
                        vscode.window.activeTextEditor.revealRange(vscodeRange, vscode.TextEditorRevealType.Default);
                        vscode.window.activeTextEditor.selection = new vscode.Selection(vscodeRange.start, vscodeRange.end);
                        vscode.commands.executeCommand('workbench.action.focusActiveEditorGroup');            
                    }
            }));
            
    }

    showSymbolsTreeView() {
        let symbolsTreeView = new SymbolsTreeView(this.context,
            this.context.activeDocumentSymbols.rootSymbol, undefined, undefined,this.context.activeDocumentSymbols.getDocUri());
        symbolsTreeView.show();
    }

}