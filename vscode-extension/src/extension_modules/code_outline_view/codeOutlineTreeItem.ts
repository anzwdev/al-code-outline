import * as vscode from 'vscode';
import { CodeOutlineSortMode } from './codeOutlineSortMode';
import { CompareHelper } from '../../core/compareHelper';
import { CodeOutlineTreeDocumentState } from './codeOutlineTreeDocumentState';
import { LSTextPositionComparer } from '../../langserver/common_types/lsTextPositionComparer';
import { CodeOutlineViewConst } from './codeOutlineViewConst';
import { LSSyntaxNodeKind } from '../../langserver/common_types/lsSyntaxNodeKind';

export class CodeOutlineTreeItem extends vscode.TreeItem {
    parent?: CodeOutlineTreeItem;
    childNodes?: CodeOutlineTreeItem[];
    selectionRange?: vscode.Range;
    range?: vscode.Range;
    kind: LSSyntaxNodeKind;

    constructor(id: string, label: string, kind: LSSyntaxNodeKind | undefined, range: vscode.Range | undefined, selectionRange: vscode.Range | undefined, iconPath: vscode.IconPath | undefined, parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState) {
        super(label);

        this.id = state.getId() + "_" + id;
        this.kind = kind ?? LSSyntaxNodeKind.Undefined;
        this.parent = parent;
        this.childNodes = undefined;
        this.range = range;
        this.selectionRange = selectionRange;
        this.iconPath = iconPath;
        this.collapsibleState = this.getDefaultCollapsibleState(state);

        if (this.selectionRange) {
            this.command = {
                command: CodeOutlineViewConst.cmdSelectOutlineNodeText,
                title: '',
                arguments: [
                    this.selectionRange
                ]
            };
        }
    }

    updateChildNodes(childNodes: CodeOutlineTreeItem[] | undefined, state: CodeOutlineTreeDocumentState){
        this.childNodes = childNodes;
        this.collapsibleState = this.getDefaultCollapsibleState(state);
    }

    sort(sortMode: CodeOutlineSortMode) {
        if (this.childNodes) {
            if (this.childNodes.length > 1) {
                switch (sortMode) {
                    case CodeOutlineSortMode.position:
                        this.childNodes.sort((a,b) => {
                            return LSTextPositionComparer.compare(a.range?.start, b.range?.start);
                        });
                        break;
                    case CodeOutlineSortMode.category:
                        this.childNodes.sort((a,b) => {
                            let aKind = a.kind;
                            let bKind = b.kind;
                            let compareResult = CompareHelper.compareValues(LSSyntaxNodeKind[aKind], LSSyntaxNodeKind[bKind]);
                            if (compareResult !== 0) {
                                return compareResult;
                            }
                            let aLabel = a.label ?? "";
                            let bLabel = b.label ?? "";
                            return CompareHelper.compareValues(aLabel.toString().toLowerCase(), bLabel.toString().toLowerCase());
                        });
                        break;
                    case CodeOutlineSortMode.name:
                        this.childNodes.sort((a,b) => {
                            return CompareHelper.compareValues(a.label!.toString().toLowerCase(), b.label!.toString().toLowerCase());
                        });
                        break;
                }
            }
            for (let i=0; i<this.childNodes.length; i++) {
                this.childNodes[i].sort(sortMode);
            }
        }
    }

    private getDefaultCollapsibleState(state: CodeOutlineTreeDocumentState): vscode.TreeItemCollapsibleState {
        if ((this.childNodes) && (this.childNodes.length > 0)) {
            switch (this.kind) {
                //AL Symbols
                case LSSyntaxNodeKind.MethodDeclaration:
                case LSSyntaxNodeKind.ParameterList:
                case LSSyntaxNodeKind.TriggerDeclaration:
                case LSSyntaxNodeKind.LocalMethodDeclaration:
                case LSSyntaxNodeKind.ProtectedMethodDeclaration:
                case LSSyntaxNodeKind.InternalMethodDeclaration:
                case LSSyntaxNodeKind.EventDeclaration:
                case LSSyntaxNodeKind.EventTriggerDeclaration:
                case LSSyntaxNodeKind.EventSubscriberDeclaration:
                case LSSyntaxNodeKind.BusinessEventDeclaration:
                case LSSyntaxNodeKind.ExternalBusinessEventDeclaration:
                case LSSyntaxNodeKind.IntegrationEventDeclaration:
                case LSSyntaxNodeKind.InternalEventDeclaration:
                case LSSyntaxNodeKind.PageHandlerDeclaration:
                case LSSyntaxNodeKind.ReportHandlerDeclaration:
                case LSSyntaxNodeKind.ConfirmHandlerDeclaration:
                case LSSyntaxNodeKind.MessageHandlerDeclaration:
                case LSSyntaxNodeKind.StrMenuHandlerDeclaration:
                case LSSyntaxNodeKind.HyperlinkHandlerDeclaration:
                case LSSyntaxNodeKind.ModalPageHandlerDeclaration:
                case LSSyntaxNodeKind.FilterPageHandlerDeclaration:
                case LSSyntaxNodeKind.RequestPageHandlerDeclaration:
                case LSSyntaxNodeKind.SessionSettingsHandlerDeclaration:
                case LSSyntaxNodeKind.SendNotificationHandlerDeclaration:
                case LSSyntaxNodeKind.TestDeclaration:
                case LSSyntaxNodeKind.Field:
                case LSSyntaxNodeKind.PageField:
                case LSSyntaxNodeKind.PageAction:
                case LSSyntaxNodeKind.PageLabel:
                case LSSyntaxNodeKind.PropertyList:
                case LSSyntaxNodeKind.VarSection:
                case LSSyntaxNodeKind.GlobalVarSection:
                case LSSyntaxNodeKind.Class:
                case LSSyntaxNodeKind.Field:
                case LSSyntaxNodeKind.Region:
                    return state.getState(this.id!, vscode.TreeItemCollapsibleState.Collapsed);
                default: 
                    return state.getState(this.id!, vscode.TreeItemCollapsibleState.Expanded);
            }
        }
        return vscode.TreeItemCollapsibleState.None;
    }

    public findNodeAtPosition(position: vscode.Position, incCurr: boolean): CodeOutlineTreeItem | undefined {
        if ((this.range) && (this.range.contains(position))) {
            if (this.childNodes) {
                for (let i=0; i<this.childNodes.length; i++) {
                    let symbol = this.childNodes[i].findNodeAtPosition(position, true);
                    if (symbol) {
                        return symbol;
                    }
                }
            }
            if (incCurr) {
                return this;
            }
        } 
        return undefined;
    }


}