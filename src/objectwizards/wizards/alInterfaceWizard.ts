import { ALObjectWizard } from "./alObjectWizard";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALInterfaceWizardData } from "./alInterfaceWizardData";
import { ALInterfaceWizardPage } from "./alIterfaceWizardPage";

export class ALInterfaceWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALInterfaceWizardData = new ALInterfaceWizardData();
        wizardData.objectName = '';//settings.getInputNameVariable();
        this.onBeforeCreateWizardPage(wizardData);
        let wizardPage : ALInterfaceWizardPage = new ALInterfaceWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

}