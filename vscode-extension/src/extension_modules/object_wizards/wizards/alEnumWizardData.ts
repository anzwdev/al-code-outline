import { ALObjectWizardData } from "./alObjectWizardData";

export class ALEnumWizardData extends ALObjectWizardData {
    objectName : string;
    valueList : string;
    captionList : string;
    extensible : boolean;
    limitNameLength : boolean;
    
    constructor() {
        super();
        this.objectName = "";
        this.valueList = "";
        this.captionList = "";
        this.extensible = true;
        this.limitNameLength = false;
    }

} 