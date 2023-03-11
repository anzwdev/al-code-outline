import * as vscode from 'vscode';

export class TypedQuickPickItem<T> implements vscode.QuickPickItem {
    label: string;    
    picked?: boolean | undefined;
    value: T;

    constructor(newLabel: string, newValue: T, newPicked: boolean) {
        this.label = newLabel;
        this.value = newValue;
        this.picked = newPicked;
    }
}