import { ToolsGetProjectSettingsResponse } from "../../langserver/toolsGetProjectSettingsResponse";
import { ObjectInformation } from "../../symbolsinformation/objectInformation";

export class ALPermissionSetWizardData {
    objectId : string;
    objectName: string;
    objectCaption: string;
    inclAllObjects: boolean;

    projectSettings: ToolsGetProjectSettingsResponse | undefined;
    permissionSetList : string[] | undefined;
    selectedPermissionSetList : string[] | undefined;
    selectedObjectsList: ObjectInformation[] | undefined;

    constructor() {
        this.objectId = '';
        this.objectName = '';
        this.objectCaption = '';
        this.inclAllObjects = true;
        this.projectSettings = undefined;
        this.permissionSetList = undefined;
        this.selectedPermissionSetList = undefined;
        this.selectedObjectsList = undefined;
    }

}