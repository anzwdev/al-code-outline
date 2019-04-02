'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { ALAbstractWizard } from "./alAbstractWizard";
import { ALLangServerProxy } from '../../allanguage/alLangServerProxy';
import { ALEnumExtWizardData } from "./alEnumExtWizardData";
import { ALEnumExtWizardPage } from "./alEnumExtWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";

export class ALEnumExtWizard extends ALAbstractWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext) {
        super(toolsExtensionContext, "azALDevTools.alEnumExtWizard");
    }

    run(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super.run(template, settings);
        this.runAsync(template, settings);
    }

    protected async runAsync(template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        let alLangProxy : ALLangServerProxy = new ALLangServerProxy();
        let objectId : string = await alLangProxy.getNextObjectId("enumextension");

        let wizardData : ALEnumExtWizardData = new ALEnumExtWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = settings.getInputNameVariable();
        let wizardPage : ALEnumExtWizardPage = new ALEnumExtWizardPage(this._toolsExtensionContext, template, settings, wizardData);
        wizardPage.show();
    }

} 