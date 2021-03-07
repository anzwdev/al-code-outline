'use strict';

import { TableFieldInformation } from "../../symbolsinformation/tableFieldInformation";
import { TableInformation } from "../../symbolsinformation/tableInformation";

export class ALTableBasedWizardData {
    objectId : string;
    objectName : string;
    tableList : string[] | undefined;
    selectedTable : string;
    fieldList : TableFieldInformation[] | undefined;
    selectedFieldList : TableFieldInformation[] | undefined;
    fixedTable : boolean;
    applicationArea : string;

    constructor() {
        this.objectId = "";
        this.objectName = "";
        this.tableList = undefined;
        this.selectedTable = "";
        this.fieldList = undefined;
        this.selectedFieldList = undefined;
        this.fixedTable = false;
        this.applicationArea = "All";
    }

}