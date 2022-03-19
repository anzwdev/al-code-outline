import * as vscode from 'vscode';
import * as path from 'path';
import { DuplicateCodeTreeNode } from './duplicateCodeTreeNode';
import { DuplicateInfo } from './duplicateInfo';
import { CodeBlockType } from './codeBlockType';
import { DuplicateCodeSortMode } from './duplicateCodeSortMode';

export class DuplicateCodeTreeProvider implements vscode.TreeDataProvider<DuplicateCodeTreeNode> {
    protected _duplicates: DuplicateCodeTreeNode[] | undefined;
    protected _sortMode: DuplicateCodeSortMode = DuplicateCodeSortMode.noOfStatements;
    private _onDidChangeTreeData: vscode.EventEmitter<DuplicateCodeTreeNode | null> = new vscode.EventEmitter<DuplicateCodeTreeNode | null>();
	readonly onDidChangeTreeData: vscode.Event<DuplicateCodeTreeNode | null> = this._onDidChangeTreeData.event;
    
    getTreeItem(element: DuplicateCodeTreeNode): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }

    getChildren(element?: DuplicateCodeTreeNode): vscode.ProviderResult<DuplicateCodeTreeNode[]> {
        if (element) {
            if ((element.childNodes) && (element.childNodes.length > 0))
                return element.childNodes;
            return undefined;
        }
        return this._duplicates;
    }
    
    setDuplicates(duplicates : DuplicateInfo[]) {
        this._duplicates = this.createDuplicatesTree(duplicates);
        this.sortDuplicates(this._sortMode, true);
    }

    sortDuplicates(sortMode: DuplicateCodeSortMode, force: boolean) {
        if ((force) || (this._sortMode != sortMode)) {
            this._sortMode = sortMode;
            if ((this._duplicates) && (this._duplicates.length > 1)) {
                this._duplicates.sort((a,b) => {
                    return this.compareDuplicateGroup(a, b);                    
                });
            }
        }      
        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire(null);
    }

    protected compareDuplicateGroup(a: DuplicateCodeTreeNode, b: DuplicateCodeTreeNode): number {
        if ((this._sortMode == DuplicateCodeSortMode.codeBlockType) || (this._sortMode == DuplicateCodeSortMode.codeBlockTypeNoOfDuplicates)) {
            if (a.codeBlockType > b.codeBlockType)
                return 1;
            if (a.codeBlockType < b.codeBlockType)
                return -1;
        }

        if ((this._sortMode == DuplicateCodeSortMode.noOfDuplicates) || (this._sortMode == DuplicateCodeSortMode.codeBlockTypeNoOfDuplicates)) {
            let aCount = a.childNodes?a.childNodes.length:0;
            let bCount = b.childNodes?b.childNodes.length:0;
            if (aCount < bCount)
                return 1;
            if (aCount > bCount)
                return -1;
        }

        if (a.noOfStatements < b.noOfStatements)
            return 1;
        if (a.noOfStatements > b.noOfStatements)
            return -1;
        return 0;
    }

    protected createDuplicatesTree(duplicates : DuplicateInfo[]): DuplicateCodeTreeNode[] {
        let duplicatesTree: DuplicateCodeTreeNode[] = [];
        for (let i=0; i<duplicates.length; i++) {
            let groupNode = this.createDuplicateTreeNodes(duplicates[i]);
            if (groupNode)
                duplicatesTree.push(groupNode);
        }
        return duplicatesTree;
    }

    protected getGroupCaption(info: DuplicateInfo): string {
        switch (info.codeBlockType) {
            case CodeBlockType.Method:
                return 'Method';
            case CodeBlockType.Trigger:
                return 'Trigger';
        }
        return 'Code';
    }

    protected createDuplicateTreeNodes(info: DuplicateInfo): DuplicateCodeTreeNode | undefined {
        if ((info.ranges) && (info.ranges.length > 1)) {
            let groupType = this.getGroupCaption(info);
            let groupTitle = groupType + ' (' + info.ranges.length.toString() + ' duplicates';
            let groupTooltip = 'Group of ' + info.ranges.length.toString() + ' duplicates';
            if (info.noOfStatements) {
                groupTitle = groupTitle + ', ' + info.noOfStatements.toString() + ' statements';
                groupTooltip = groupTooltip + '\n' + info.noOfStatements.toString() + ' statements in each duplicated range';
            }
            groupTitle = groupTitle + ')';

            let node = new DuplicateCodeTreeNode(groupTitle, vscode.TreeItemCollapsibleState.Collapsed, info.noOfStatements!, info.codeBlockType!);
            node.tooltip = groupTooltip;

            let childNodes: DuplicateCodeTreeNode[] = [];
            for (let i=0; i<info.ranges.length; i++) {
                let filePath: string = (info.ranges[i].filePath)?info.ranges[i].filePath!:'file';
                let fileName = path.parse(filePath).name;
                let codeBlockTree = new DuplicateCodeTreeNode(fileName, vscode.TreeItemCollapsibleState.None, info.noOfStatements!, info.codeBlockType!);
                codeBlockTree.tooltip = filePath;
                codeBlockTree.parent = node;
                codeBlockTree.documentRange = info.ranges[i];
                if (info.ranges[i].filePath)
                    codeBlockTree.resourceUri = vscode.Uri.file(filePath);
                codeBlockTree.command = {
                    title: "Show code",
                    command: "azALDevTools.showDuplicateCode",
                    arguments: [ codeBlockTree.documentRange ]
                };
                childNodes.push(codeBlockTree);
            }
            node.childNodes = childNodes;
            return node;
        }
        return undefined;
    }

    getParent(element: DuplicateCodeTreeNode): DuplicateCodeTreeNode | undefined {
        return element.parent;
    }

    getFirstDuplicateNode() : DuplicateCodeTreeNode | undefined {
        if ((this._duplicates) && (this._duplicates.length > 0))
            return this._duplicates[0];
        return undefined;
    }
}