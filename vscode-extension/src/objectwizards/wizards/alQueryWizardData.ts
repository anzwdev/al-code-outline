'use strict';

import { ALTableBasedWizardData } from "./alTableBasedWizardData";

export class ALQueryWizardData extends ALTableBasedWizardData {
    createRequestPage : boolean;
    queryType : string;
    apiPublisher : string;
    apiGroup : string;
    apiVersion : string;
    entityName : string;
    entitySetName : string;

    constructor() {
        super();
        this.createRequestPage = false;
        this.queryType = "Normal";
        this.apiPublisher = "publisherName";
        this.apiGroup = "apiGroup";
        this.apiVersion = "v1.0";
        this.entityName = "entityName";
        this.entitySetName = "entitySetName"
    }

} 