import * as vscode from 'vscode';
import { CodeCompletionTextEdit } from './codeCompletionTextEdit';

export class CodeCompletionItem {
    label?: string;
    kind?: vscode.CompletionItemKind;
    filterText?: string;
    tags?: vscode.CompletionItemTag[];
    insertText?: string;
    detail?: string;
    description?: string;
    commitCharacters?: string[];
    additionalTextEdits?: CodeCompletionTextEdit[];
}