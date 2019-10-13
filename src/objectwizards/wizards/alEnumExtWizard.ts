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
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId(settings.getDestDirectoryUri(), "enumextension");

        let wizardData : ALEnumExtWizardData = new ALEnumExtWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = '';
        let wizardPage : ALEnumExtWizardPage = new ALEnumExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 