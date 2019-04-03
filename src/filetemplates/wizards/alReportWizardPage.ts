'use strict';

import * as path from 'path';
import * as vzFileTemplates from 'vz-file-templates';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALReportWizardData } from './alReportWizardData';
import { ALReportSyntaxBuilder } from '../syntaxbuilders/alReportSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALReportWizardPage extends ALTableBasedWizardPage {
    protected _reportWizardData : ALReportWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings, data : ALReportWizardData) {
        super(toolsExtensionContext, "AL Report Wizard", template, settings, data);
        this._reportWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alreportwizard', 'alreportwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALReportWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._reportWizardData.objectId = data.objectId;
        this._reportWizardData.objectName = data.objectName;
        this._reportWizardData.selectedTable = data.selectedTable;
        this._reportWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._reportWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        //build new object
        var builder : ALReportSyntaxBuilder = new ALReportSyntaxBuilder();
        var source = builder.buildFromReportWizardData(this._reportWizardData);

        await this.setObjectFileName("report", data.objectId, data.objectName);

        //run template
        this._templateRunSettings.setTextReplacement("$wizardoutput$", source);
        this._template.run(this._templateRunSettings);

        return true;
    }

}
