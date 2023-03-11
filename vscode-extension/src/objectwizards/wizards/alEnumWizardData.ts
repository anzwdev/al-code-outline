import { ALObjectWizardData } from "./alObjectWizardData";

export class ALEnumWizardData extends ALObjectWizardData {
    objectName : string;
    valueList : string;
    captionList : string;
    extensible : boolean;
    
    constructor() {
        super();
        this.objectName = "";
        this.valueList = "";
        this.captionList = "";
        this.extensible = true;
    }

} 