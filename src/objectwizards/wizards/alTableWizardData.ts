'use strict';

import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { ALTableWizardFieldData } from "./alTableWizardFieldData";

export class ALTableWizardData {
    objectId : string;
    objectName : string;
    fields: ALTableWizardFieldData[];
    projectSettings: ToolsGetProjectSettingsResponse | undefined;

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.fields = [];
        this.projectSettings = undefined;
    }

}