import { ALPermissionSetWizardData } from "./alPermissionSetWizardData";

export class ALPermissionSetExtensionWizardData extends ALPermissionSetWizardData {
    basePermissionSet: string;

    constructor() {
        super();
        this.basePermissionSet = "";
    }

}