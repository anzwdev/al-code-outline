'use strict';

import { ALTableBasedWizardData } from "./alTableBasedWizardData";

export class ALReportWizardData extends ALTableBasedWizardData {
    createRequestPage : boolean;
    rdlcLayout : string;
    wordLayout : string;

    constructor() {
        super();
        this.createRequestPage = true;
        this.rdlcLayout = "";
        this.wordLayout = "";
    }

} 