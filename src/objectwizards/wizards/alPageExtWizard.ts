import { ALObjectWizard } from "./alObjectWizard";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPageExtWizardPage } from "./alPageExtWizardPage";
import { ALPageExtWizardData } from "./alPageExtWizardData";

export class ALPageExtWizard extends ALObjectWizard {

    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALPageExtWizardData = new ALPageExtWizardData();
        this.initObjectIdFields(wizardData, settings, "pageextension");
        wizardData.objectName = '';
        wizardData.basePage = '';
        let wizardPage : ALPageExtWizardPage = new ALPageExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

}