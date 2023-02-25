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
        let wizardData : ALReportWizardData = new ALReportWizardData();
        this.initObjectIdFields(wizardData, settings, "Report");
        wizardData.objectName = '';
        
        //build relative path
        wizardData.rdlcLayout = '';

        this.onBeforeCreateWizardPage(wizardData);
        let wizardPage : ALReportWizardPage = new ALReportWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 