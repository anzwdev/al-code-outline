export class ALReportExtWizardData {
    objectId : string;
    objectName : string;
    reportList : string[] | undefined;
    baseReport: string;

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.reportList = undefined;
        this.baseReport = "";
    }
}
