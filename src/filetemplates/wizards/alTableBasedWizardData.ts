'use strict';

export class ALTableBasedWizardData {
    objectId : string;
    objectName : string;
    tableList : string[] | undefined;
    selectedTable : string;
    fieldList : string[] | undefined;
    selectedFieldList : string[] | undefined;
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