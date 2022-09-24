import * as vscode from 'vscode';
import * as path from 'path';
import { WarningDirectiveInfo } from '../symbolsinformation/warningDirectiveInfo';
import { WarningDirectiveInfoKind } from '../symbolsinformation/warningDirectiveInfoKind';
import { WarningDirectivesTreeNode } from './warningDirectivesTreeNode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';

export class WarningDirectivesTreeProvider implements vscode.TreeDataProvider<WarningDirectivesTreeNode> {
    private _toolsExtensionContext : DevToolsExtensionContext;
    private _warningDirectives?: WarningDirectivesTreeNode[];
    
    private _onDidChangeTreeData: vscode.EventEmitter<WarningDirectivesTreeNode | null> = new vscode.EventEmitter<WarningDirectivesTreeNode | null>();
	readonly onDidChangeTreeData: vscode.Event<WarningDirectivesTreeNode | null> = this._onDidChangeTreeData.event;
   
    constructor(context : DevToolsExtensionContext) {
        this._toolsExtensionContext = context;        
    }

    getTreeItem(element: WarningDirectivesTreeNode): vscode.TreeItem | Thenable<vscode.TreeItem> {
        return element;
    }

    getChildren(element?: WarningDirectivesTreeNode): vscode.ProviderResult<WarningDirectivesTreeNode[]> {
        if (element) {
            if ((element.childNodes) && (element.childNodes.length > 0))
                return element.childNodes;
            return undefined;
        }
        return this._warningDirectives;
    }

    getParent?(element: WarningDirectivesTreeNode): vscode.ProviderResult<WarningDirectivesTreeNode> {
        return element.parent;
    }

    setWarningDirectives(directivesInfo?: WarningDirectiveInfo[]) {
        this._warningDirectives = this.createDirectivesTree(directivesInfo);
        if (this._onDidChangeTreeData)
            this._onDidChangeTreeData.fire(null);
    }

    private createDirectivesTree(directivesInfo?: WarningDirectiveInfo[]) : WarningDirectivesTreeNode[] | undefined {
        if ((directivesInfo) && (directivesInfo.length == 1) && (directivesInfo[0].kind == WarningDirectiveInfoKind.Project))
            directivesInfo = directivesInfo[0].childItems;
        
        if (!directivesInfo)
            return undefined;
        let directives: WarningDirectivesTreeNode[] = [];

        for (let i=0; i<directivesInfo.length; i++)
            directives.push(this.createTreeNode(directivesInfo[i], undefined));

        return directives;
    }

    private getNodeIcon(info: WarningDirectiveInfo) : { light: string, dark: string } | undefined {
        switch (info.kind) {
            case WarningDirectiveInfoKind.Project:
                return this.getIcon("tree-project.svg");
            case WarningDirectiveInfoKind.Rule:
                return this.getIcon("tree-rule.svg");
            case WarningDirectiveInfoKind.File:
                return this.getIcon("tree-file.svg");
            case WarningDirectiveInfoKind.DirectiveLocation:
                if (info.disabled)
                    return this.getIcon("tree-ruledisabled.svg");
                return this.getIcon("tree-ruleenabled.svg");
        }

        return undefined;
    }

    private getIcon(fileName: string) : { light: string, dark: string } {
        return {
            light: this._toolsExtensionContext.vscodeExtensionContext.asAbsolutePath(path.join("resources", "images", "light", fileName)),
            dark: this._toolsExtensionContext.vscodeExtensionContext.asAbsolutePath(path.join("resources", "images", "dark", fileName))
        };
    }

    private createTreeNode(info: WarningDirectiveInfo, parent?: WarningDirectivesTreeNode) : WarningDirectivesTreeNode {
        let directiveNode = new WarningDirectivesTreeNode(
            info.title!, 
            (info.childItems)?vscode.TreeItemCollapsibleState.Collapsed:vscode.TreeItemCollapsibleState.None);
        directiveNode.directiveInfo = info;
        directiveNode.parent = parent;
        if (info.fullPath)
            directiveNode.resourceUri = vscode.Uri.file(info.fullPath);
        directiveNode.iconPath = this.getNodeIcon(directiveNode.directiveInfo);

        if (info.childItems) {
            directiveNode.childNodes = [];
            for (let i=0; i<info.childItems.length; i++)
                directiveNode.childNodes.push(this.createTreeNode(info.childItems[i], directiveNode));
        }

        if ((info.kind == WarningDirectiveInfoKind.File) || (info.kind == WarningDirectiveInfoKind.DirectiveLocation)) {
            directiveNode.command = {
                title: "Show code",
                command: "azALDevTools.showWarningDirectiveItem",
                arguments: [ directiveNode ]
            };
        }

        return directiveNode;
    }

    getFirstNode(): WarningDirectivesTreeNode | undefined {
        if ((this._warningDirectives) && (this._warningDirectives.length > 0))
            return this._warningDirectives[0];
        return undefined;
    }

}