import { ALObjectWizardData } from "./alObjectWizardData";

export class ALInterfaceWizardData extends ALObjectWizardData {
    objectName : string;
    baseCodeunitName: string;
    codeunitList: string[] | undefined;

    constructor() {
        super();
        this.objectName = '';
        this.baseCodeunitName = '';
        this.codeunitList = undefined;
    }

}