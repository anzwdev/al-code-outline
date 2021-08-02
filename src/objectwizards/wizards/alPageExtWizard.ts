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
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "pageextension");

        let wizardData : ALPageExtWizardData = new ALPageExtWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        wizardData.basePage = '';
        let wizardPage : ALPageExtWizardPage = new ALPageExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

}