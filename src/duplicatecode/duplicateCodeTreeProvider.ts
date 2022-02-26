import * as vscode from 'vscode';
import { DuplicateCodeTreeNode } from './duplicateCodeTreeNode';
import { DuplicateInfo } from './duplicateInfo';

export class DuplicateCodeTreeProvider implements vscode.TreeDataProvider<DuplicateCodeTreeNode> {
    protected _duplicates: DuplicateCodeTreeNode[] | undefined;

    private _onDidChangeTreeData: vscode.EventEmitter<DuplicateCodeTreeNode | null> = new vscode.EventEmitter<DuplicateCodeTreeNode | null>();
	readonly onDidChangeTreeData: vscode.Event<DuplicateCodeTreeNode | null> = this._onDidChangeTreeData.event;
    
    getTreeItem(element: DuplicateCodeTreeNode): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }

    getChildren(element?: DuplicateCodeTreeNode): vscode.ProviderResult<DuplicateCodeTreeNode[]> {
        if (element)
            return element.childNodes;
        return this._duplicates;
    }
    
    setDuplicates(duplicates : DuplicateInfo[]) {
        this._duplicates = this.createDuplicatesTree(duplicates);
        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire(null);
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

    protected createDuplicateTreeNodes(info: DuplicateInfo): DuplicateCodeTreeNode | undefined {
        if ((info.ranges) && (info.ranges.length > 1)) {
            let node = new DuplicateCodeTreeNode('Group (' + info.ranges.length.toString() + 'blocks)', vscode.TreeItemCollapsibleState.Collapsed);
            let childNodes: DuplicateCodeTreeNode[] = [];
            for (let i=0; i<info.ranges.length; i++) {
                let title: string = (info.ranges[i].filePath)?info.ranges[i].filePath!:'file';
                let codeBlockTree = new DuplicateCodeTreeNode(title, vscode.TreeItemCollapsibleState.Collapsed);
                codeBlockTree.documentRange = info.ranges[i];
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
}