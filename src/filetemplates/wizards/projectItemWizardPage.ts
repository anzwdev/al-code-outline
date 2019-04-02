'use strict';

import * as vzFileTemplates from 'vz-file-templates';
import { BaseWebViewEditor } from '../../webviews/baseWebViewEditor';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

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

    protected finishWizard(data : any) : boolean {
        return false;
    }

    protected onFinish(data : any) {
        if (this.finishWizard(data))
            this.close();
    }

    protected onCancel() {
        this.close();
    }

}