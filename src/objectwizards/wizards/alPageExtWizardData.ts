export class ALPageExtWizardData {
    objectId : string;
    objectName : string;
    pageList : string[] | undefined;
    basePage: string;

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.pageList = undefined;
        this.basePage = "";
    }

}