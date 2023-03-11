import * as vscode from 'vscode';
import { DocumentTextRange } from '../symbollibraries/documentTextRange';
import { WarningDirectiveInfo } from '../symbolsinformation/warningDirectiveInfo';

export class WarningDirectivesTreeNode extends vscode.TreeItem {
    parent?: WarningDirectivesTreeNode;
    directiveInfo?: WarningDirectiveInfo;
    childNodes?: WarningDirectivesTreeNode[];
}