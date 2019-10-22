'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALPageWizardData } from "./alPageWizardData";
import { ALPageWizardPage } from "./alPageWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALPageWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : string = await this._toolsExtensionContext.alLangProxy.getNextObjectId(settings.getDestDirectoryUri(), "Page");

        let wizardData : ALPageWizardData = new ALPageWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = '';//settings.getInputNameVariable();
        let wizardPage : ALPageWizardPage = new ALPageWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 