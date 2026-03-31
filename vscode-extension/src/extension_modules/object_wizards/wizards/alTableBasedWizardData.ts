import { ALAppAreaMode } from "../../extension_settings/alAppAreaMode";
import { ALObjectWizardData } from "./alObjectWizardData";
import { LSPITableFieldListItem } from "../../../langserver/project_information/symbols/lspiTableFieldListItem";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";

export class ALTableBasedWizardData extends ALObjectWizardData {
    objectName : string;
    tableList : LSPIObjectListItem[] | undefined;
    selectedTable : LSPIObjectIdentifier | undefined;
    fieldList : LSPITableFieldListItem[] | undefined;
    flowFiltersList : LSPITableFieldListItem[] | undefined;
    selectedFieldList : LSPITableFieldListItem[] | undefined;
    selectedFlowFilterList : LSPITableFieldListItem[] | undefined;
    fixedTable : boolean;
    applicationArea : string;
    applicationAreaMode : ALAppAreaMode;

    constructor() {
        super();
        this.objectName = "";
        this.tableList = undefined;
        this.selectedTable = undefined;
        this.fieldList = undefined;
        this.selectedFieldList = undefined;
        this.fixedTable = false;
        this.applicationArea = "All";
        this.applicationAreaMode = ALAppAreaMode.addToAllControls;
    }

}