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
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId(settings.getDestDirectoryUri(), "table");

        let wizardData : ALTableWizardData = new ALTableWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = '';
        let wizardPage : ALTableWizardPage = new ALTableWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }


}