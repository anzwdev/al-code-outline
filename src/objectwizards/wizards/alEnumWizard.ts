'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALEnumWizardData } from "./alEnumWizardData";
import { ALEnumWizardPage } from "./alEnumWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALEnumWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALEnumWizardData = new ALEnumWizardData();
        this.initObjectIdFields(wizardData, settings, "enum");
        wizardData.objectName = '';
        this.onBeforeCreateWizardPage(wizardData);
        let wizardPage : ALEnumWizardPage = new ALEnumWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 