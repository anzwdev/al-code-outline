'use strict';

import { ALTableBasedWizardData } from "./alTableBasedWizardData";

export class ALReportWizardData extends ALTableBasedWizardData {
    createRequestPage : boolean;
    rdlcLayout : string;
    wordLayout : string;
    usageCategory : string;

    constructor() {
        super();
        this.createRequestPage = true;
        this.rdlcLayout = "";
        this.wordLayout = "";
        this.usageCategory = "";
    }

} 