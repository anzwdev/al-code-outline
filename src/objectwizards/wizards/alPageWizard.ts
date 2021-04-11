'use strict';

import * as vscode from 'vscode';
import { ALObjectWizard } from "./alObjectWizard";
import { ALPageWizardData } from "./alPageWizardData";
import { ALPageWizardPage } from "./alPageWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";

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
        let objectId : string = await this._toolsExtensionContext.alLangProxy.getNextObjectId(settings.getDestDirectoryUri(), "Page");

        let wizardData : ALPageWizardData = new ALPageWizardData();
        wizardData.objectId = objectId;
        wizardData.objectName = '';//settings.getInputNameVariable();
        wizardData.createTooltips = !!config.get<boolean>('addToolTipsToPageFields');

        let wizardPage : ALPageWizardPage = new ALPageWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 