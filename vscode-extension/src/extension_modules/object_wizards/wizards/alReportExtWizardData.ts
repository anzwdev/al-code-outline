import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { ALObjectWizardData } from "./alObjectWizardData";
import { ALReportExtWizardDataItemData } from "./alReportExtWizardDataItemData";

export class ALReportExtWizardData extends ALObjectWizardData {
    objectName : string;
    reportList? : LSPIObjectListItem[];
    baseReport?: LSPIObjectIdentifier;
    dataItems?: ALReportExtWizardDataItemData[];

    constructor() {
        super();
        this.objectName = '';
        this.reportList = undefined;
        this.baseReport = undefined;
        this.dataItems = undefined;
    }
}
