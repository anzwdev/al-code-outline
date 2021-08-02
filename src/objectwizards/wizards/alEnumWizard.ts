'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
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
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "enum");

        let wizardData : ALEnumWizardData = new ALEnumWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        let wizardPage : ALEnumWizardPage = new ALEnumWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 