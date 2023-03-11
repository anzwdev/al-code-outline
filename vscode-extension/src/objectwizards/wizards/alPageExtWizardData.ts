import { ALObjectWizardData } from "./alObjectWizardData";

export class ALPageExtWizardData extends ALObjectWizardData {
    objectName : string;
    pageList : string[] | undefined;
    basePage: string;

    constructor() {
        super();
        this.objectName = '';
        this.pageList = undefined;
        this.basePage = "";
    }

}