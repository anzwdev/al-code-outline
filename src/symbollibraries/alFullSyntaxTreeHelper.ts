import { ALFullSyntaxTreeNode } from "./alFullSyntaxTreeNode";

export class ALFullSyntaxTreeHelper {

    static restoreNodeParent(node: ALFullSyntaxTreeNode, parentNode: ALFullSyntaxTreeNode) {
        if (node) {
            node.parentNode = parentNode;
            this.restoreNodeListParent(node.childNodes, node);
            this.restoreNodeListParent(node.attributes, node);
            this.restoreNodeParent(node.openBraceToken, node);
            this.restoreNodeParent(node.closeBraceToken, node);
            this.restoreNodeParent(node.varKeyword, node);
        }
    }

    static restoreNodeListParent(nodeList: ALFullSyntaxTreeNode[], parentNode: ALFullSyntaxTreeNode) {
        if (nodeList)
            for (let i=0; i<nodeList.length; i++)
                this.restoreNodeParent(nodeList[i], parentNode);
    }



}