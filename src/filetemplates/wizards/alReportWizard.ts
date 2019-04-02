'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ALAbstractWizard } from "./alAbstractWizard";
import { ALReportWizardData } from './alReportWizardData';
import { ALReportWizardPage } from './alReportWizardPage';
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALReportWizard extends ALAbstractWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        super(toolsExtensionContext, "azALDevTools.alReportWizard");
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super.run(template, settings);
        this.runAsync(template, settings);
    }

    protected async runAsync(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId("Report");

        let wizardData : ALReportWizardData = new ALReportWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = settings.getInputNameVariable();
        
        //build relative path
        wizardData.rdlcLayout = settings.getVariable("filerelpath") + ".rdlc";

        let wizardPage : ALReportWizardPage = new ALReportWizardPage(this._toolsExtensionContext, template, settings, wizardData);
        wizardPage.show();
    }

} 