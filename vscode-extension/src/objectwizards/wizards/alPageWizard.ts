'use strict';

import * as vscode from 'vscode';
import { ALObjectWizard } from "./alObjectWizard";
import { ALPageWizardData } from "./alPageWizardData";
import { ALPageWizardPage } from "./alPageWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { StringHelper } from '../../tools/stringHelper';
import { ALFieldToolTipsLocation } from '../../allanguage/alFieldToolTipsLocation';

export class ALPageWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let uri = settings.getDestDirectoryUri();
        let fieldToolTipsLocation = this._toolsExtensionContext.alLangProxy.fieldToolTipsLocation(uri);
        let config = vscode.workspace.getConfiguration('alOutline', uri);
        let wizardData : ALPageWizardData = new ALPageWizardData();
        await this.initObjectIdFieldsAsync(wizardData, settings, "Page");
        wizardData.objectName = '';//settings.getInputNameVariable();
        wizardData.showCreateTooltips = (fieldToolTipsLocation === ALFieldToolTipsLocation.page);
        wizardData.createTooltips = (wizardData.showCreateTooltips) && (!!config.get<boolean>('addToolTipsToPageFields'));
        wizardData.reuseToolTips = !config.get<boolean>('doNotReuseToolTipsFromOtherPages');
        wizardData.toolTipsSource = config.get<string[]>('reuseToolTipsFromDependencies');
        wizardData.applicationArea = StringHelper.defaultIfEmpty(config.get<string>('defaultAppArea'), wizardData.applicationArea);
        wizardData.apiPublisher = StringHelper.defaultIfEmpty(config.get<string>('defaultApiPublisher'), wizardData.apiPublisher);
        wizardData.apiGroup = StringHelper.defaultIfEmpty(config.get<string>('defaultApiGroup'), wizardData.apiGroup);
        wizardData.apiVersion = StringHelper.defaultIfEmpty(config.get<string>('defaultApiVersion'), wizardData.apiVersion);

        this.onInitWizardData(wizardData);
        let wizardPage : ALPageWizardPage = new ALPageWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 