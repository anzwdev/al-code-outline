'use strict';

export class ALEnumWizardData {
    objectId : string;
    objectName : string;
    valueList : string;
    captionList : string;
    extensible : boolean;
    
    constructor() {
        this.objectId = "";
        this.objectName = "";
        this.valueList = "";
        this.captionList = "";
        this.extensible = true;
    }

} 