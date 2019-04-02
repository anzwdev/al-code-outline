'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ALAbstractWizard } from "./alAbstractWizard";
import { ALXmlPortWizardData } from './alXmlPortWizardData';
import { ALXmlPortWizardPage } from './alXmlPortWizardPage';
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALXmlPortWizard extends ALAbstractWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        super(toolsExtensionContext, "azALDevTools.alXmlPortWizard");
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super.run(template, settings);
        this.runAsync(template, settings);
    }

    protected async runAsync(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId("XmlPort");

        let wizardData : ALXmlPortWizardData = new ALXmlPortWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = settings.getInputNameVariable();
        let wizardPage : ALXmlPortWizardPage = new ALXmlPortWizardPage(this._toolsExtensionContext, template, settings, wizardData);
        wizardPage.show();
    }

} 