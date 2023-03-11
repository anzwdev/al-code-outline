'use strict';

//import * as vscode from 'vscode';
import { ALTableBasedWizardData } from "./alTableBasedWizardData";
import { ALTableWizardFieldData } from "./alTableWizardFieldData";

export class ALTableExtWizardData extends ALTableBasedWizardData {
    fields: ALTableWizardFieldData[];
    idRangeStart: number;

    constructor() {
        super();
        this.fields = [];
        this.idRangeStart = 0;
    }
}
