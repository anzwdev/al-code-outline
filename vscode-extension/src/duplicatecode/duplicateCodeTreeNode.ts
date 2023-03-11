import * as vscode from 'vscode';
import { DocumentTextRange } from '../symbollibraries/documentTextRange';
import { CodeBlockType } from './codeBlockType';

export class DuplicateCodeTreeNode extends vscode.TreeItem {
    parent: DuplicateCodeTreeNode | undefined;
    documentRange: DocumentTextRange | undefined;
    childNodes: DuplicateCodeTreeNode[] | undefined;
    noOfStatements: number;
    codeBlockType: CodeBlockType;

    constructor(label: string, collapsibleState: vscode.TreeItemCollapsibleState,  newNoOfStatements: number, newCodeBlockType: CodeBlockType) {
        super(label, collapsibleState);
        this.codeBlockType = newCodeBlockType;
        this.noOfStatements = newNoOfStatements;
    }

}