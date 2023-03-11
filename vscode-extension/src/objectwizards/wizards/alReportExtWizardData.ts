import { ALObjectWizardData } from "./alObjectWizardData";
import { ALReportExtWizardDataItemData } from "./alReportExtWizardDataItemData";

export class ALReportExtWizardData extends ALObjectWizardData {
    objectName : string;
    reportList : string[] | undefined;
    baseReport: string;
    dataItems: ALReportExtWizardDataItemData[] | undefined;

    constructor() {
        super();
        this.objectName = '';
        this.reportList = undefined;
        this.baseReport = "";
        this.dataItems = undefined;
    }
}
