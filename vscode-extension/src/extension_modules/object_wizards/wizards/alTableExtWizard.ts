import { ALObjectWizard } from "./alObjectWizard";
import { ALTableExtWizardData } from "./alTableExtWizardData";
import { ALTableExtWizardPage } from "./alTableExtWizardPage";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { DevToolsExtensionContext } from "../../../devToolsExtensionContext";

export class ALTableExtWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALTableExtWizardData = new ALTableExtWizardData();
        
        await this.initObjectIdFieldsAsync(wizardData, settings, "tableextension");

        let profile = await this._toolsExtensionContext.projectInformationService.getProjectProfile(settings.destDirectoryUri);

        wizardData.objectName = "";
        wizardData.selectedTable = undefined;
        wizardData.idRangeStart = profile?.properties?.firstIdRangeStart ?? 0;
        this.onInitWizardData(wizardData);
        let wizardPage : ALTableExtWizardPage = new ALTableExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 