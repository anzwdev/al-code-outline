import { ALObjectWizard } from "./alObjectWizard";
import { ALEnumExtWizardData } from "./alEnumExtWizardData";
import { ALEnumExtWizardPage } from "./alEnumExtWizardPage";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { DevToolsExtensionContext } from "../../../devToolsExtensionContext";

export class ALEnumExtWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALEnumExtWizardData = new ALEnumExtWizardData();
        await this.initObjectIdFieldsAsync(wizardData, settings, "enumextension");
        let projectProfile = await this._toolsExtensionContext.projectInformationService.getProjectProfile(settings.destDirectoryUri);
        wizardData.objectName = '';
        wizardData.firstValueId = projectProfile?.properties?.firstIdRangeStart ?? 0;
        this.onInitWizardData(wizardData);
        let wizardPage : ALEnumExtWizardPage = new ALEnumExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 