'use strict';

import { ALTableWizardFieldData } from "./alTableWizardFieldData";

export class ALTableWizardData {
    objectId : string;
    objectName : string;
    fields: ALTableWizardFieldData[];

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.fields = [];
    }

}