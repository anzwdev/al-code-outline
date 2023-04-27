'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALXmlPortWizardData } from './alXmlPortWizardData';
import { ALXmlPortWizardPage } from './alXmlPortWizardPage';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALXmlPortWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALXmlPortWizardData = new ALXmlPortWizardData();
        await this.initObjectIdFieldsAsync(wizardData, settings, "XmlPort");
        wizardData.objectName = '';
        this.onInitWizardData(wizardData);
        let wizardPage : ALXmlPortWizardPage = new ALXmlPortWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 