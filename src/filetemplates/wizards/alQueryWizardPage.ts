'use strict';

import * as path from 'path';
import * as vzFileTemplates from 'vz-file-templates';
import { ALTableBasedWizardPage } from "./alTableBasedWizardPage";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALQuerySyntaxBuilder } from '../syntaxbuilders/alQuerySyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALQueryWizardPage extends ALTableBasedWizardPage {
    protected _queryWizardData : ALQueryWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings, data : ALQueryWizardData) {
        super(toolsExtensionContext, "AL Query Wizard", template, settings, data);
        this._queryWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alquerywizard', 'alquerywizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALQueryWizard";
    }

    protected finishWizard(data : any) : boolean {
        //build parameters
        this._queryWizardData.objectId = data.objectId;
        this._queryWizardData.objectName = data.objectName;
        this._queryWizardData.selectedTable = data.selectedTable;
        this._queryWizardData.queryType = data.queryType;
        this._queryWizardData.apiPublisher = data.apiPublisher;
        this._queryWizardData.apiGroup = data.apiGroup;
        this._queryWizardData.apiVersion = data.apiVersion;
        this._queryWizardData.entityName = data.entityName;
        this._queryWizardData.entitySetName = data.entitySetName;

        this._queryWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._queryWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        //build new object
        var builder : ALQuerySyntaxBuilder = new ALQuerySyntaxBuilder();
        var source = builder.buildFromQueryWizardData(this._queryWizardData);

        //run template
        this._templateRunSettings.setTextReplacement("$wizardoutput$", source);
        this._template.run(this._templateRunSettings);

        return true;
    }

} 