import * as vscode from 'vscode';

export class EditorSymbolsTreeItem {
    id?: string;                    // Id of the symbol, it is not unique in the tree, different nodes can have the same id (e.g. table field id, table id, etc.)
    uid?: string;                   // Unique id of the symbol in the tree
    idx?: number;                   // Index of the symbol in the list of parent node's children
    name?: string;                  // Name of the symbol
    fullName?: string;              // Full name of the symbol, can be used for display or for searching
    range?: vscode.Range;           // Range of the symbol in the document
    selectionRange?: vscode.Range;  // Range to select when the symbol is selected in the tree, if not set, the range property will be used
}
