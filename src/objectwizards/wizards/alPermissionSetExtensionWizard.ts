import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizard } from "./alObjectWizard";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPermissionSetExtensionWizardData } from "./alPermissionSetExtensionWizardData";
import { ALPermissionSetExtensionWizardPage } from "./alPermissionSetExtensionWizardPage";

export class ALPermissionSetExtensionWizard extends ALObjectWizard {
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "permissionsetextension");

        let wizardData : ALPermissionSetExtensionWizardData = new ALPermissionSetExtensionWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        let wizardPage : ALPermissionSetExtensionWizardPage = new ALPermissionSetExtensionWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

}