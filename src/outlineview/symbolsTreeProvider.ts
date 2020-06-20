import * as vscode from 'vscode';
import * as path from 'path';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { AZSymbolsLibrary } from '../symbollibraries/azSymbolsLibrary';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';

export class SymbolsTreeProvider implements vscode.TreeDataProvider<AZSymbolInformation> {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _treeRoot : AZSymbolInformation | undefined;

	private _onDidChangeTreeData: vscode.EventEmitter<AZSymbolInformation | null> = new vscode.EventEmitter<AZSymbolInformation | null>();
	readonly onDidChangeTreeData: vscode.Event<AZSymbolInformation | null> = this._onDidChangeTreeData.event;

    constructor(context : DevToolsExtensionContext) {
        this._toolsExtensionContext = context;        
        this._toolsExtensionContext.activeDocumentSymbols.onSymbolsChanged(symbolsLib => this.onSymbolsChanged(symbolsLib));
    }

    protected onSymbolsChanged(library : AZSymbolsLibrary) {
        this._treeRoot = library.rootSymbol;        
        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire();
    }

    public refresh() {
        this._toolsExtensionContext.activeDocumentSymbols.loadAsync(true);
    }

    protected getSymbolIcon(symbol : AZSymbolInformation) : any {
        let icon = "tree-" + symbol.icon + ".svg";
        
        return {
            light: this._toolsExtensionContext.vscodeExtensionContext.asAbsolutePath(path.join("resources", "images", "light", icon)),
            dark: this._toolsExtensionContext.vscodeExtensionContext.asAbsolutePath(path.join("resources", "images", "dark", icon))
        }

    }

    protected getNodeCollapsibleState(element: AZSymbolInformation) {
        if ((element.childSymbols) && (element.childSymbols.length > 0)) {
            switch (element.kind) {
                //AL Symbols
                case AZSymbolKind.MethodDeclaration:
                case AZSymbolKind.ParameterList:
                case AZSymbolKind.TriggerDeclaration:
                case AZSymbolKind.LocalMethodDeclaration:
                case AZSymbolKind.EventDeclaration:
                case AZSymbolKind.EventTriggerDeclaration:
                case AZSymbolKind.PageHandlerDeclaration:
                case AZSymbolKind.ReportHandlerDeclaration:
                case AZSymbolKind.ConfirmHandlerDeclaration:
                case AZSymbolKind.MessageHandlerDeclaration:
                case AZSymbolKind.StrMenuHandlerDeclaration:
                case AZSymbolKind.HyperlinkHandlerDeclaration:
                case AZSymbolKind.ModalPageHandlerDeclaration:
                case AZSymbolKind.FilterPageHandlerDeclaration:
                case AZSymbolKind.RequestPageHandlerDeclaration:
                case AZSymbolKind.SessionSettingsHandlerDeclaration:
                case AZSymbolKind.SendNotificationHandlerDeclaration:
                case AZSymbolKind.TestDeclaration:
                case AZSymbolKind.Field:
                case AZSymbolKind.PageField:
                case AZSymbolKind.PageAction:
                //Other Symbols
                case AZSymbolKind.Class:
                case AZSymbolKind.Field:
                    return vscode.TreeItemCollapsibleState.Collapsed;
                default: 
                    return vscode.TreeItemCollapsibleState.Expanded;
            }
        } else
            return vscode.TreeItemCollapsibleState.None;
    }

    //#region TreeDataProvider implementation

    getTreeItem(element: AZSymbolInformation): vscode.TreeItem | Thenable<vscode.TreeItem> {
        let treeItem = new vscode.TreeItem(element.fullName);
        treeItem.iconPath = this.getSymbolIcon(element);
        treeItem.collapsibleState = this.getNodeCollapsibleState(element);
        
            //node command
        if (element.selectionRange) 
            treeItem.command = {
                command: 'azALDevTools.selectDocumentText',
                title: '',
                arguments: [
                    element.selectionRange
                ]
            };
        //node context
        treeItem.contextValue = AZSymbolKind[element.kind];

        return treeItem;
    }
    
    async getChildren(element?: AZSymbolInformation): Promise<AZSymbolInformation[]> {
        if ((element) && (element.childSymbols))
            return element.childSymbols;
        if ((!element) && (this._treeRoot) && (this._treeRoot.childSymbols))
            return this._treeRoot.childSymbols;
        return [];
    }

    //#endregion

}