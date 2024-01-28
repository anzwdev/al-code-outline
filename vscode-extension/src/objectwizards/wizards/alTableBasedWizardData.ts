'use strict';

import { AppAreaMode } from "../../alsyntaxmodifiers/appAreaMode";
import { ToolsSymbolReference } from "../../langserver/symbolsinformation/toolsSymbolReference";
import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { TableFieldInformation } from "../../symbolsinformation/tableFieldInformation";
import { ALObjectWizardData } from "./alObjectWizardData";

export class ALTableBasedWizardData extends ALObjectWizardData {
    objectName : string;
    tableList : string[] | undefined;
    selectedTable : string | undefined;
    fieldList : TableFieldInformation[] | undefined;
    flowFiltersList : TableFieldInformation[] | undefined;
    selectedFieldList : TableFieldInformation[] | undefined;
    selectedFlowFilterList : TableFieldInformation[] | undefined;
    fixedTable : boolean;
    applicationArea : string;
    applicationAreaMode : AppAreaMode;
    projectSettings: ToolsGetProjectSettingsResponse | undefined;

    constructor() {
        super();
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