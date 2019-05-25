'use strict';

import { ALTableBasedWizardData } from "./alTableBasedWizardData";

export class ALXmlPortWizardData extends ALTableBasedWizardData {
    fieldNodeType : string;
    createRequestPage : boolean;

    constructor() {
        super();
        this.fieldNodeType = "element";
        this.createRequestPage = true;
    }

} 