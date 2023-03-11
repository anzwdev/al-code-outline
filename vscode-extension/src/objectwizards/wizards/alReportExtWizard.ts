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
        let wizardData : ALReportExtWizardData = new ALReportExtWizardData();
        this.initObjectIdFields(wizardData, settings, "reportextension");
        wizardData.objectName = '';
        wizardData.baseReport = '';
        this.onInitWizardData(wizardData);
        let wizardPage : ALReportExtWizardPage = new ALReportExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

}