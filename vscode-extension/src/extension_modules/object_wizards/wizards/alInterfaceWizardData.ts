import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { LSPIObjectListItem } from "../../../langserver/project_information/symbols/lspiObjectListItem";
import { ALObjectWizardData } from "./alObjectWizardData";

export class ALInterfaceWizardData extends ALObjectWizardData {
    objectName : string;
    baseCodeunit?: LSPIObjectIdentifier;
    codeunitList: LSPIObjectListItem[] | undefined;

    constructor() {
        super();
        this.objectName = '';
        this.baseCodeunit = undefined;
        this.codeunitList = undefined;
    }

}