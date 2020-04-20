import { TextRange } from "./textRange";

export class ALFullSyntaxTreeNode {
    name: string | undefined;
    kind: string | undefined;
    span: TextRange | undefined;
    fullSpan: TextRange | undefined;
    childNodes: ALFullSyntaxTreeNode[] | undefined;
    parentNode: ALFullSyntaxTreeNode | undefined;

    attributes: ALFullSyntaxTreeNode[] | undefined;
    openBraceToken: ALFullSyntaxTreeNode | undefined;
    closeBraceToken: ALFullSyntaxTreeNode | undefined;
    varKeyword: ALFullSyntaxTreeNode | undefined;
    accessModifier: string | undefined;
    identifier: string | undefined;
    dataType: string | undefined;
    temporary: string | undefined;
}