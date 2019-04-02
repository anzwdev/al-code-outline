'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ALAbstractWizard } from "./alAbstractWizard";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { ALEnumWizardData } from "./alEnumWizardData";
import { ALEnumWizardPage } from "./alEnumWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";

export class ALEnumWizard extends ALAbstractWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        super(toolsExtensionContext, "azALDevTools.alEnumWizard");
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super.run(template, settings);
        this.runAsync(template, settings);
    }

    protected async runAsync(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId("enum");

        let wizardData : ALEnumWizardData = new ALEnumWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = settings.getInputNameVariable();
        let wizardPage : ALEnumWizardPage = new ALEnumWizardPage(this._toolsExtensionContext, template, settings, wizardData);
        wizardPage.show();
    }

} 