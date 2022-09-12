import * as vscode from 'vscode';

export class CodeCompletionItem {
    label?: string;
    kind?: vscode.CompletionItemKind;
    filterText?: string;
    tags?: vscode.CompletionItemTag[];
    insertText?: string;
    detail?: string;
}