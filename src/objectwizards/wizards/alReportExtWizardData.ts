import { ALReportExtWizardDataItemData } from "./alReportExtWizardDataItemData";

export class ALReportExtWizardData {
    objectId : string;
    objectName : string;
    reportList : string[] | undefined;
    baseReport: string;
    dataItems: ALReportExtWizardDataItemData[] | undefined;

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.reportList = undefined;
        this.baseReport = "";
        this.dataItems = undefined;
    }
}
