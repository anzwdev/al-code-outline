import * as vscode from 'vscode';

export class FieldQuickPickItem implements vscode.QuickPickItem {
    label: string;    
    description?: string | undefined;
    detail?: string | undefined;
    picked?: boolean | undefined;
    alwaysShow?: boolean | undefined;

    constructor(name: string) {
        this.label = name;
    }

}