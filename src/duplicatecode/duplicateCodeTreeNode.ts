import * as vscode from 'vscode';
import { DocumentTextRange } from '../symbollibraries/documentTextRange';

export class DuplicateCodeTreeNode extends vscode.TreeItem {
    documentRange: DocumentTextRange | undefined;
    childNodes: DuplicateCodeTreeNode[] | undefined;
}