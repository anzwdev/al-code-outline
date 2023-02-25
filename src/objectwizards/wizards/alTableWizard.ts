'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALLangServerProxy } from "../../allanguage/alLangServerProxy";
import { ALTableWizardData } from "./alTableWizardData";
import { ALTableWizardPage } from "./alTableWizardPage";

export class ALTableWizard extends ALObjectWizard {

    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALTableWizardData = new ALTableWizardData();
        this.initObjectIdFields(wizardData, settings, "table");
        wizardData.objectName = '';
        this.onBeforeCreateWizardPage(wizardData);
        let wizardPage : ALTableWizardPage = new ALTableWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }


}