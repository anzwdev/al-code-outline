'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALQueryWizardPage } from "./alQueryWizardPage";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALQueryWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "Query");
        
        let wizardData : ALQueryWizardData = new ALQueryWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        let wizardPage : ALQueryWizardPage = new ALQueryWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }


} 