import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { ALObjectWizardData } from "./alObjectWizardData";

export class ALPermissionSetWizardData extends ALObjectWizardData {
    objectName: string;
    objectCaption: string;
    inclAllObjects: boolean;

    permissionSetList : LSPIObjectListItem[] | undefined;
    selectedPermissionSetList : LSPIObjectIdentifier[] | undefined;
    selectedObjectsList: LSPIObjectIdentifier[] | undefined;

    constructor() {
        super();
        this.objectName = '';
        this.objectCaption = '';
        this.inclAllObjects = true;
        this.permissionSetList = undefined;
        this.selectedPermissionSetList = undefined;
        this.selectedObjectsList = undefined;
    }

}