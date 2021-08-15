import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizard } from "./alObjectWizard";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPermissionSetWizardData } from "./alPermissionSetWizardData";
import { ALPermissionSetWizardPage } from "./alPermissionSetWizardPage";

export class ALPermissionSetWizard extends ALObjectWizard {
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "permissionset");

        let wizardData : ALPermissionSetWizardData = new ALPermissionSetWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        let wizardPage : ALPermissionSetWizardPage = new ALPermissionSetWizardPage(this._toolsExtensionContext, undefined, settings, wizardData);
        wizardPage.show();
    }

   
}