import * as vscode from 'vscode';
import { ALObjectWizard } from "./alObjectWizard";
import { ALEnumWizardData } from "./alEnumWizardData";
import { ALEnumWizardPage } from "./alEnumWizardPage";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';

export class ALEnumWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let vscodeSettings = vscode.workspace.getConfiguration('alOutline', settings.destDirectoryUri);
        let wizardData : ALEnumWizardData = new ALEnumWizardData();

        wizardData.limitNameLength = !!vscodeSettings.get<boolean>('limitEnumNameLength');

        await this.initObjectIdFieldsAsync(wizardData, settings, "enum");
        
        wizardData.objectName = '';
        this.onInitWizardData(wizardData);
        let wizardPage : ALEnumWizardPage = new ALEnumWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 