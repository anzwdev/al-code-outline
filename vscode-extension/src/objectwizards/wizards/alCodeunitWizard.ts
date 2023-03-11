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
        let wizardData : ALCodeunitWizardData = new ALCodeunitWizardData();
        this.initObjectIdFields(wizardData, settings, "Codeunit");
        wizardData.objectName = '';//settings.getInputNameVariable();
        this.onInitWizardData(wizardData);
        let wizardPage : ALCodeunitWizardPage = new ALCodeunitWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }


}