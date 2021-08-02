import { ALObjectWizard } from "./alObjectWizard";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALCodeunitWizardData } from "./alCodeunitWizardData";
import { ALCodeunitWizardPage } from "./alCodeunitWizardPage";

export class ALCodeunitWizard extends ALObjectWizard {

    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "Codeunit");

        let wizardData : ALCodeunitWizardData = new ALCodeunitWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';//settings.getInputNameVariable();
        let wizardPage : ALCodeunitWizardPage = new ALCodeunitWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }


}