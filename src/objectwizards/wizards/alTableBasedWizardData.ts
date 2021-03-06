'use strict';

import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { TableFieldInformation } from "../../symbolsinformation/tableFieldInformation";

export class ALTableBasedWizardData {
    objectId : string;
    objectName : string;
    tableList : string[] | undefined;
    selectedTable : string;
    fieldList : TableFieldInformation[] | undefined;
    selectedFieldList : TableFieldInformation[] | undefined;
    fixedTable : boolean;
    applicationArea : string;
    projectSettings: ToolsGetProjectSettingsResponse | undefined;

    constructor() {
        this.objectId = "";
        this.objectName = "";
        this.tableList = undefined;
        this.selectedTable = "";
        this.fieldList = undefined;
        this.selectedFieldList = undefined;
        this.fixedTable = false;
        this.applicationArea = "All";
        this.projectSettings = undefined;
    }

}