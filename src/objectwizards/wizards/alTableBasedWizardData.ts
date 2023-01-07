'use strict';

import { AppAreaMode } from "../../alsyntaxmodifiers/appAreaMode";
import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { TableFieldInformation } from "../../symbolsinformation/tableFieldInformation";

export class ALTableBasedWizardData {
    objectId : string;
    objectName : string;
    tableList : string[] | undefined;
    selectedTable : string;
    fieldList : TableFieldInformation[] | undefined;
    flowFiltersList : TableFieldInformation[] | undefined;
    selectedFieldList : TableFieldInformation[] | undefined;
    selectedFlowFilterList : TableFieldInformation[] | undefined;
    fixedTable : boolean;
    applicationArea : string;
    applicationAreaMode : AppAreaMode;
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
        this.applicationAreaMode = AppAreaMode.addToAllControls;
        this.projectSettings = undefined;
    }

}