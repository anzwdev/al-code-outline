import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { ObjectInformation } from "../../symbolsinformation/objectInformation";
import { ALObjectWizardData } from "./alObjectWizardData";

export class ALPermissionSetWizardData extends ALObjectWizardData {
    objectName: string;
    objectCaption: string;
    inclAllObjects: boolean;

    projectSettings: ToolsGetProjectSettingsResponse | undefined;
    permissionSetList : string[] | undefined;
    selectedPermissionSetList : string[] | undefined;
    selectedObjectsList: ObjectInformation[] | undefined;

    constructor() {
        super();
        this.objectName = '';
        this.objectCaption = '';
        this.inclAllObjects = true;
        this.projectSettings = undefined;
        this.permissionSetList = undefined;
        this.selectedPermissionSetList = undefined;
        this.selectedObjectsList = undefined;
    }

}