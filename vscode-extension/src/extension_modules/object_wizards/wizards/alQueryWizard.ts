import * as vscode from 'vscode';
import { ALObjectWizard } from "./alObjectWizard";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALQueryWizardPage } from "./alQueryWizardPage";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { StringHelper } from '../../../core/stringHelper';
import { DevToolsExtensionSettings } from '../../extension_settings/devToolsExtensionSettings';

export class ALQueryWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let extensionSettings = new DevToolsExtensionSettings(settings.destDirectoryUri);

        let wizardData : ALQueryWizardData = new ALQueryWizardData();
        await this.initObjectIdFieldsAsync(wizardData, settings, "Query");
        wizardData.objectName = '';
        wizardData.apiPublisher = StringHelper.defaultIfEmpty(extensionSettings.getDefaultApiPublisher(), wizardData.apiPublisher);
        wizardData.apiGroup = StringHelper.defaultIfEmpty(extensionSettings.getDefaultApiGroup(), wizardData.apiGroup);
        wizardData.apiVersion = StringHelper.defaultIfEmpty(extensionSettings.getDefaultApiVersion(), wizardData.apiVersion);
        this.onInitWizardData(wizardData);
        let wizardPage : ALQueryWizardPage = new ALQueryWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 