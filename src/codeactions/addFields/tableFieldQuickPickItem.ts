import * as vscode from 'vscode';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';

export class TableFieldQuickPickItem implements vscode.QuickPickItem {
    fieldId: number;
    fieldInformation: TableFieldInformation;
    label: string;
    description?: string | undefined;
    detail?: string | undefined;
    picked?: boolean | undefined;
    alwaysShow?: boolean | undefined;

    constructor(newFieldInformation: TableFieldInformation) {
        this.fieldInformation = newFieldInformation;
        if (newFieldInformation.id)
            this.fieldId = newFieldInformation.id;
        else
            this.fieldId = 0;
        if (newFieldInformation.name)
            this.label = newFieldInformation.name;
        else
            this.label = '';
    }

}