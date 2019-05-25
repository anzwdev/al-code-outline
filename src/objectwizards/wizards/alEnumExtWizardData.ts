import { ALEnumWizardData } from "./alEnumWizardData";

'use strict';

export class ALEnumExtWizardData extends ALEnumWizardData {
    firstValueId : number;
    baseEnum : string;
    baseEnumList : string[];
    
    constructor() {
        super();
        this.firstValueId = 0;
        this.baseEnum = "";
        this.baseEnumList = [];
    }

} 