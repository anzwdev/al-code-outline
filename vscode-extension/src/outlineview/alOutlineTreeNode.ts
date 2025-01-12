import * as vscode from 'vscode';
import * as path from 'path';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALOutlineSortMode } from './alOutlineSortMode';
import { ALOutlineTreeDocumentState } from './alOutlineTreeDocumentState';

export class ALOutlineTreeItem extends vscode.TreeItem {
    parent: ALOutlineTreeItem | undefined;
    childNodes: ALOutlineTreeItem[] | undefined;
    symbol: AZSymbolInformation;

    constructor(newSymbol: AZSymbolInformation, context: vscode.ExtensionContext, newParent: ALOutlineTreeItem | undefined, state: ALOutlineTreeDocumentState, idPrefix: string, index: number) {
        super(newSymbol.fullName);
        this.symbol = newSymbol;
        this.parent = newParent;       
        this.id = idPrefix + '_' + index.toString();
        this.createChildNodes(context, state, this.id);
        this.updateIcon(context);

        this.collapsibleState = this.getDefaultCollapsibleState(state);
        this.contextValue = AZSymbolKind[this.symbol.kind];
        if (this.symbol.selectionRange) 
            this.command = {
                command: 'azALDevTools.selectDocumentText',
                title: '',
                arguments: [
                    this.symbol.selectionRange
                ]
            };
    }

    sort(sortMode: ALOutlineSortMode) {
        if (this.childNodes) {
            if (this.childNodes.length > 1)
                switch (sortMode) {
                    case ALOutlineSortMode.position:
                        this.childNodes.sort((a,b) => {
                            if ((a.symbol.range) && (b.symbol.range))
                                return a.symbol.range.start.compare(b.symbol.range.start);
                            return 0;                               
                        });
                        break;
                    case ALOutlineSortMode.category:
                        this.childNodes.sort((a,b) => {
                            let aname = AZSymbolKind[a.symbol.kind];
                            let bname = AZSymbolKind[b.symbol.kind];
                            if (aname < bname) return -1;
                            if (aname > bname) return 1;

                            aname = a.label!.toString().toLowerCase();
                            bname = b.label!.toString().toLowerCase();
                            if (aname < bname) return -1;
                            if (aname > bname) return 1;
                            return 0;                               
                        });
                        break;
                    case ALOutlineSortMode.name:
                        this.childNodes.sort((a,b) => {
                            let aname = a.label!.toString().toLowerCase();
                            let bname = b.label!.toString().toLowerCase();
                            if (aname < bname) return -1;
                            if (aname > bname) return 1;
                            return 0;                               
                        });
                        break;
                }
            for (let i=0; i<this.childNodes.length; i++) {
                this.childNodes[i].sort(sortMode);
            }
        }
    }

    private createChildNodes(context: vscode.ExtensionContext, state: ALOutlineTreeDocumentState, idPrefix: string) {
        if (this.symbol.childSymbols) {
            this.childNodes = [];
            for (let i=0; i<this.symbol.childSymbols.length; i++) {
                let item = new ALOutlineTreeItem(this.symbol.childSymbols[i], context, this, state, idPrefix, i);
                this.childNodes.push(item);
            }
        }
    }

    private updateIcon(context: vscode.ExtensionContext) {
        let icon = "tree-" + this.symbol.icon + ".svg";
        this.iconPath = {
            light: context.asAbsolutePath(path.join("resources", "images", "light", icon)),
            dark: context.asAbsolutePath(path.join("resources", "images", "dark", icon))
        }
    }

    private getDefaultCollapsibleState(state: ALOutlineTreeDocumentState): vscode.TreeItemCollapsibleState {
        if ((this.childNodes) && (this.childNodes.length > 0)) {
            switch (this.symbol.kind) {
                //AL Symbols
                case AZSymbolKind.MethodDeclaration:
                case AZSymbolKind.ParameterList:
                case AZSymbolKind.TriggerDeclaration:
                case AZSymbolKind.LocalMethodDeclaration:
                case AZSymbolKind.ProtectedMethodDeclaration:
                case AZSymbolKind.InternalMethodDeclaration:
                case AZSymbolKind.EventDeclaration:
                case AZSymbolKind.EventTriggerDeclaration:
                case AZSymbolKind.EventSubscriberDeclaration:
                case AZSymbolKind.BusinessEventDeclaration:
                case AZSymbolKind.ExternalBusinessEventDeclaration:
                case AZSymbolKind.IntegrationEventDeclaration:
                case AZSymbolKind.InternalEventDeclaration:
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
                case AZSymbolKind.PageLabel:
                case AZSymbolKind.PropertyList:
                case AZSymbolKind.VarSection:
                case AZSymbolKind.GlobalVarSection:
                case AZSymbolKind.Class:
                case AZSymbolKind.Field:
                case AZSymbolKind.Region:
                    return state.getState(this.id!, vscode.TreeItemCollapsibleState.Collapsed);
                default: 
                    return state.getState(this.id!, vscode.TreeItemCollapsibleState.Expanded);
            }
        } else
            return vscode.TreeItemCollapsibleState.None;
    }

    public findNodeAtPosition(position: vscode.Position, incCurr: boolean): ALOutlineTreeItem | undefined {
        if ((this.symbol.range) && 
            (this.symbol.range.start.compareVsPosition(position) <= 0) && 
            (this.symbol.range.end.compareVsPosition(position) >= 0)) {

            if (this.childNodes) {
                for (let i=0; i<this.childNodes.length; i++) {
                    let symbol = this.childNodes[i].findNodeAtPosition(position, true);
                    if (symbol)
                        return symbol;
                }
            }
            if (incCurr)
                return this;
        } 
        return undefined;
    }

    public saveState(state: ALOutlineTreeDocumentState) {
        if ((this.childNodes) && (this.childNodes.length > 0) && (this.collapsibleState !== undefined)) {
            state.setState(this.id!, this.collapsibleState);
            for (let i=0; i<this.childNodes.length; i++)
                this.childNodes[i].saveState(state);
        }
    }
}