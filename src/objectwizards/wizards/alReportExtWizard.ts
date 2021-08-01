import { ALObjectWizard } from "./alObjectWizard";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALReportExtWizardPage } from "./alReportExtWizardPage";
import { ALReportExtWizardData } from "./alReportExtWizardData";

export class ALReportExtWizard extends ALObjectWizard {

    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        // Currently there is a bug in the AL compiler: https://github.com/microsoft/AL/issues/6689
        let objectId : string = await this._toolsExtensionContext.alLangProxy.getNextObjectId(settings.getDestDirectoryUri(), "reportextension");

        let wizardData : ALReportExtWizardData = new ALReportExtWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = '';
        wizardData.baseReport = '';
        let wizardPage : ALReportExtWizardPage = new ALReportExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

}