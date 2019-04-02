'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ALAbstractWizard } from "./alAbstractWizard";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALQueryWizardPage } from "./alQueryWizardPage";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALQueryWizard extends ALAbstractWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        super(toolsExtensionContext, "azALDevTools.alQueryWizard");
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super.run(template, settings);
        this.runAsync(template, settings);
    }

    protected async runAsync(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId("Query");
        
        let wizardData : ALQueryWizardData = new ALQueryWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = settings.getInputNameVariable();
        let wizardPage : ALQueryWizardPage = new ALQueryWizardPage(this._toolsExtensionContext, template, settings, wizardData);
        wizardPage.show();
    }


} 