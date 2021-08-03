'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALReportWizardData } from './alReportWizardData';
import { ALReportWizardPage } from './alReportWizardPage';
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALReportWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "Report");

        let wizardData : ALReportWizardData = new ALReportWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';
        
        //build relative path
        wizardData.rdlcLayout = '';

        let wizardPage : ALReportWizardPage = new ALReportWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 