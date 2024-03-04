'use strict';

import * as vscode from 'vscode';
import { ALObjectWizard } from "./alObjectWizard";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALQueryWizardPage } from "./alQueryWizardPage";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { StringHelper } from "../../tools/stringHelper";

export class ALQueryWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let config = vscode.workspace.getConfiguration('alOutline', settings.getDestDirectoryUri());

        let wizardData : ALQueryWizardData = new ALQueryWizardData();
        await this.initObjectIdFieldsAsync(wizardData, settings, "Query");
        wizardData.objectName = '';
        wizardData.apiPublisher = StringHelper.defaultIfEmpty(config.get<string>('defaultApiPublisher'), wizardData.apiPublisher);
        wizardData.apiGroup = StringHelper.defaultIfEmpty(config.get<string>('defaultApiGroup'), wizardData.apiGroup);
        wizardData.apiVersion = StringHelper.defaultIfEmpty(config.get<string>('defaultApiVersion'), wizardData.apiVersion);

        this.onInitWizardData(wizardData);
        let wizardPage : ALQueryWizardPage = new ALQueryWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 