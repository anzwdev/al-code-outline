import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { ALObjectWizardData } from "./alObjectWizardData";
import { ALTableWizardFieldData } from "./alTableWizardFieldData";

export class ALTableWizardData extends ALObjectWizardData {
    objectName : string;
    dataPerCompany : boolean;
    fields: ALTableWizardFieldData[];
    dataClassification: string | undefined;
    projectSettings: ToolsGetProjectSettingsResponse | undefined;

    constructor() {
        super();
        this.objectName = '';
        this.dataClassification = "ToBeClassified";
        this.dataPerCompany = true;
        this.fields = [];
        this.projectSettings = undefined;
    }

}