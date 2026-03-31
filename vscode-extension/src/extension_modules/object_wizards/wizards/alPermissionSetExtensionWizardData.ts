import { LSPIObjectIdentifier } from "../../../langserver/project_information/symbols/lspiObjectIdentifier";
import { ALPermissionSetWizardData } from "./alPermissionSetWizardData";

export class ALPermissionSetExtensionWizardData extends ALPermissionSetWizardData {
    basePermissionSet?: LSPIObjectIdentifier;

    constructor() {
        super();
        this.basePermissionSet = undefined;
    }

}