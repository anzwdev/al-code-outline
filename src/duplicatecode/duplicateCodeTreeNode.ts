import * as vscode from 'vscode';
import { DocumentTextRange } from '../symbollibraries/documentTextRange';

export class DuplicateCodeTreeNode extends vscode.TreeItem {
    parent: DuplicateCodeTreeNode | undefined;
    documentRange: DocumentTextRange | undefined;
    childNodes: DuplicateCodeTreeNode[] | undefined;
}