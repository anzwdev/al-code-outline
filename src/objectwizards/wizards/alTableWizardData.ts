'use strict';

import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { ALTableWizardFieldData } from "./alTableWizardFieldData";

export class ALTableWizardData {
    objectId : string;
    objectName : string;
    dataPerCompany : boolean;
    fields: ALTableWizardFieldData[];
    projectSettings: ToolsGetProjectSettingsResponse | undefined;

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.dataPerCompany = true;
        this.fields = [];
        this.projectSettings = undefined;
    }

}