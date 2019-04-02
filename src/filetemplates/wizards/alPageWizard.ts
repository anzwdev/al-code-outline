'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ALAbstractWizard } from "./alAbstractWizard";
import { ALPageWizardData } from "./alPageWizardData";
import { ALPageWizardPage } from "./alPageWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";

export class ALPageWizard extends ALAbstractWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        super(toolsExtensionContext, "azALDevTools.alPageWizard");
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super.run(template, settings);
        this.runAsync(template, settings);
    }

    protected async runAsync(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        let objectId : string = await this._toolsExtensionContext.alLangProxy.getNextObjectId("Page");

        let wizardData : ALPageWizardData = new ALPageWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = settings.getInputNameVariable();
        let wizardPage : ALPageWizardPage = new ALPageWizardPage(this._toolsExtensionContext, template, settings, wizardData);
        wizardPage.show();
    }

} 