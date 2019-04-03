'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { BaseWebViewEditor } from '../../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { CRSALLangExtHelper } from '../../crsAlLangExtHelper';
import { ICRSExtensionPublicApi } from 'crs-al-language-extension-api';

export class ProjectItemWizardPage extends BaseWebViewEditor {
    protected _toolsExtensionContext : DevToolsExtensionContext;
    protected _template : vzFileTemplates.IProjectItemTemplate;
    protected _templateRunSettings : vzFileTemplates.IProjectItemTemplateRunSettings;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title : string, template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings) {
        super(toolsExtensionContext.vscodeExtensionContext, title);
        this._toolsExtensionContext = toolsExtensionContext;
        this._template = template;
        this._templateRunSettings = settings;
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

        switch (message.command) {
            case 'finishClick':
                this.onFinish(message.data);
                return true;
            case 'cancelClick':
                this.onCancel();
                return true;
        }
        
        return false;
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        return false;
    }

    protected async onFinish(data : any) {
        if (await this.finishWizard(data))
            this.close();
    }

    protected onCancel() {
        this.close();
    }

    protected async setObjectFileName(objectType : string, objectId : string, objectName : string) {
        let crsExtensionApi : ICRSExtensionPublicApi | undefined = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsExtensionApi)
            this.setObjectFileNameVariable(crsExtensionApi.ObjectNamesApi.GetObjectFileName(objectType, objectId, objectName));
        else
            this.setObjectFileNameVariable(objectName + ".al");
    }

    protected async setExtObjectFileName(objectType : string, objectId : string, objectName : string, extendedObjectName : string) {
        let crsExtensionApi : ICRSExtensionPublicApi | undefined = await CRSALLangExtHelper.GetCrsAlLangExt();
        if (crsExtensionApi)
            this.setObjectFileNameVariable(crsExtensionApi.ObjectNamesApi.GetObjectExtensionFileName(
                objectType, objectId, objectName, "", extendedObjectName));
        else
            this.setObjectFileNameVariable(objectName + ".al");
    }

    protected setObjectFileNameVariable(name : string) {
        this._templateRunSettings.setVariable("objectfilename", name);
    }

}