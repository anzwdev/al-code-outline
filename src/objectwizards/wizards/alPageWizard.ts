'use strict';

import * as vscode from 'vscode';
import { ALObjectWizard } from "./alObjectWizard";
import { ALPageWizardData } from "./alPageWizardData";
import { ALPageWizardPage } from "./alPageWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { StringHelper } from '../../tools/stringHelper';

export class ALPageWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let config = vscode.workspace.getConfiguration('alOutline', settings.getDestDirectoryUri());
        let objectId : number = await this._toolsExtensionContext.toolsLangServerClient.getNextObjectId(settings.getDestDirectoryPath(), "Page");

        let wizardData : ALPageWizardData = new ALPageWizardData();
        wizardData.objectId = objectId.toString();
        wizardData.objectName = '';//settings.getInputNameVariable();
        wizardData.createTooltips = !!config.get<boolean>('addToolTipsToPageFields');
        wizardData.reuseToolTips = !config.get<boolean>('doNotReuseToolTipsFromOtherPages');
        wizardData.toolTipsSource = config.get<string[]>('reuseToolTipsFromDependencies');
        wizardData.applicationArea = StringHelper.defaultIfEmpty(config.get<string>('defaultAppArea'), wizardData.applicationArea);
        wizardData.apiPublisher = StringHelper.defaultIfEmpty(config.get<string>('defaultApiPublisher'), wizardData.apiPublisher);
        wizardData.apiGroup = StringHelper.defaultIfEmpty(config.get<string>('defaultApiGroup'), wizardData.apiGroup);
        wizardData.apiVersion = StringHelper.defaultIfEmpty(config.get<string>('defaultApiVersion'), wizardData.apiVersion);

        let wizardPage : ALPageWizardPage = new ALPageWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 