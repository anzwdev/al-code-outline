import * as vscode from 'vscode';

export class NameValueQuickPickItem implements vscode.QuickPickItem {
    label: string;    
    picked?: boolean | undefined;
    value: string;

    constructor(newLabel: string, newValue: string, newPicked: boolean) {
        this.label = newLabel;
        this.value = newValue;
        this.picked = newPicked;
    }

}