'use strict';

import { TableFieldInformation } from "../../symbolsinformation/tableFieldInformation";

export class ALPageWizardFastTabData {
    name : string;
    fields : TableFieldInformation[];

    constructor(newName : string) {
        this.name = newName;
        this.fields = [];
    }

}
