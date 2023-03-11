'use strict';

import { ALObjectWizard } from "./alObjectWizard";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { ALTableExtWizardData } from "./alTableExtWizardData";
import { ALTableExtWizardPage } from "./alTableExtWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

export class ALTableExtWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let wizardData : ALTableExtWizardData = new ALTableExtWizardData();
        this.initObjectIdFields(wizardData, settings, "tableextension");
        wizardData.objectName = '';
        wizardData.selectedTable = '';
        wizardData.idRangeStart = 
            this._toolsExtensionContext.alLangProxy.getIdRangeStart(settings.getDestDirectoryUri());
        this.onInitWizardData(wizardData);
        let wizardPage : ALTableExtWizardPage = new ALTableExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 