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
        let alLangProxy : ALLangServerProxy = this._toolsExtensionContext.alLangProxy;
        let objectId : string = await alLangProxy.getNextObjectId(settings.getDestDirectoryUri(), "tableextension");

        let wizardData : ALTableExtWizardData = new ALTableExtWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = '';
        wizardData.selectedTable = '';
        wizardData.idRangeStart = 
            alLangProxy.getIdRangeStart(settings.getDestDirectoryUri());
        let wizardPage : ALTableExtWizardPage = new ALTableExtWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 