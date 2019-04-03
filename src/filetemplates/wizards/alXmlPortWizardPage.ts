'use strict';

import * as path from 'path';
import * as vzFileTemplates from 'vz-file-templates';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALXmlPortWizardData } from './alXmlPortWizardData';
import { ALXmlPortSyntaxBuilder } from '../syntaxbuilders/alXmlPortSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALXmlPortWizardPage extends ALTableBasedWizardPage {
    protected _xmlPortWizardData : ALXmlPortWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings, data : ALXmlPortWizardData) {
        super(toolsExtensionContext, "AL XmlPort Wizard", template, settings, data);
        this._xmlPortWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alxmlportwizard', 'alxmlportwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALXmlPortWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._xmlPortWizardData.objectId = data.objectId;
        this._xmlPortWizardData.objectName = data.objectName;
        this._xmlPortWizardData.selectedTable = data.selectedTable;
        this._xmlPortWizardData.fieldNodeType = data.fieldNodeType;
        this._xmlPortWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._xmlPortWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        //build new object
        var builder : ALXmlPortSyntaxBuilder = new ALXmlPortSyntaxBuilder();
        var source = builder.buildFromXmlPortWizardData(this._xmlPortWizardData);

        await this.setObjectFileName("xmlport", data.objectId, data.objectName);

        //run template
        this._templateRunSettings.setTextReplacement("$wizardoutput$", source);
        this._template.run(this._templateRunSettings);

        return true;
    }

}
