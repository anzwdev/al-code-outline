'use strict';

import * as path from 'path';
import * as vzFileTemplates from 'vz-file-templates';
import { ALPageWizardData } from './alPageWizardData';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALPageWizardFastTabData } from './alPageWizardFastTabData';
import { ALPageSyntaxBuilder } from '../syntaxbuilders/alPageSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';

export class ALPageWizardPage extends ALTableBasedWizardPage {
    protected _pageWizardData : ALPageWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, template : vzFileTemplates.IProjectItemTemplate, settings : vzFileTemplates.IProjectItemTemplateRunSettings, data : ALPageWizardData) {
        super(toolsExtensionContext, "AL Page Wizard", template, settings, data);
        this._pageWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alpagewizard', 'alpagewizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALPageWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._pageWizardData.objectId = data.objectId;
        this._pageWizardData.objectName = data.objectName;
        this._pageWizardData.selectedTable = data.selectedTable;
        this._pageWizardData.pageType = data.pageType;
        this._pageWizardData.fastTabs = data.fastTabs;
        this._pageWizardData.appArea = data.appArea;
        this._pageWizardData.usageCategory = data.usageCategory;
        this._pageWizardData.caption = data.caption;
        this._pageWizardData.apiPublisher = data.apiPublisher;
        this._pageWizardData.apiGroup = data.apiGroup;
        this._pageWizardData.apiVersion = data.apiVersion;
        this._pageWizardData.entityName = data.entityName;
        this._pageWizardData.entitySetName = data.entitySetName;
    
        //information about selected fields
        this._pageWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._pageWizardData.selectedFieldList.push(data.fields[i]);
            }
        }
        //information about fast tabs
        this._pageWizardData.fastTabsData = [];
        if (data.fastTabsData) {
            for (var i = 0; i<data.fastTabsData.length; i++) {
                var sourceFastTabDetails = data.fastTabsData[i];
                var fastTabDetails = new ALPageWizardFastTabData(sourceFastTabDetails.name);
                if (sourceFastTabDetails.fields) {
                    for (var fld = 0; fld < sourceFastTabDetails.fields.length; fld++)
                        fastTabDetails.fields.push(sourceFastTabDetails.fields[fld]);
                }
                this._pageWizardData.fastTabsData.push(fastTabDetails);
            }
        }

        //build new object
        var builder : ALPageSyntaxBuilder = new ALPageSyntaxBuilder();
        var source = builder.buildFromPageWizardData(this._pageWizardData);

        await this.setObjectFileName("page", data.objectId, data.objectName);

        //run template
        this._templateRunSettings.setTextReplacement("$wizardoutput$", source);
        this._template.run(this._templateRunSettings);

        return true;
    }

}
