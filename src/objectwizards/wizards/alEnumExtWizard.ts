'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { ALEnumExtWizardData } from "./alEnumExtWizardData";
import { ALEnumExtWizardPage } from "./alEnumExtWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALEnumExtWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "enumextension");

        let wizardData : ALEnumExtWizardData = new ALEnumExtWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        let wizardPage : ALEnumExtWizardPage = new ALEnumExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 