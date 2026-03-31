import { ALObjectWizard } from "./alObjectWizard";
import { ALPageWizardData } from "./alPageWizardData";
import { ALPageWizardPage } from "./alPageWizardPage";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { StringHelper } from '../../../core/stringHelper';
import { MSALLanguageServerSettings } from '../../ms_al_langserver/msalLanguageServerSettings';
import { DevToolsExtensionSettings } from '../../extension_settings/devToolsExtensionSettings';
import { ALFieldToolTipsLocation } from '../../extension_settings/alFieldToolTipsLocation';

export class ALPageWizard extends ALObjectWizard {
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, newLabel: string, newDescription : string, newDetails: string) {
        super(toolsExtensionContext, newLabel, newDescription, newDetails);
    }

    run(settings: ALObjectWizardSettings) {
        super.run(settings);
        this.runAsync(settings);
    }

    protected async runAsync(settings: ALObjectWizardSettings) {
        let alSettings = new MSALLanguageServerSettings(settings.destDirectoryUri);
        let devToolsSettings = new DevToolsExtensionSettings(settings.destDirectoryUri);

        let fieldToolTipsLocation = devToolsSettings.getFieldToolTipsLocation(settings.projectProfile?.platformCapabilities);

        let wizardData : ALPageWizardData = new ALPageWizardData();
        await this.initObjectIdFieldsAsync(wizardData, settings, "Page");

        wizardData.objectName = "";//settings.getInputNameVariable();
        wizardData.showCreateTooltips = (fieldToolTipsLocation === ALFieldToolTipsLocation.page);
        wizardData.createTooltips = (wizardData.showCreateTooltips) && (!!devToolsSettings.getAddToolTipsToPageFields());
        wizardData.reuseToolTips = !devToolsSettings.getDoNotReuseToolTipsFromOtherPages();
        wizardData.toolTipsSource = devToolsSettings.getReuseToolTipsFromDependencies();
        wizardData.applicationArea = StringHelper.defaultIfEmpty(devToolsSettings.getDefaultAppArea(), wizardData.applicationArea);
        wizardData.apiPublisher = StringHelper.defaultIfEmpty(devToolsSettings.getDefaultApiPublisher(), wizardData.apiPublisher);
        wizardData.apiGroup = StringHelper.defaultIfEmpty(devToolsSettings.getDefaultApiGroup(), wizardData.apiGroup);
        wizardData.apiVersion = StringHelper.defaultIfEmpty(devToolsSettings.getDefaultApiVersion(), wizardData.apiVersion);

        this.onInitWizardData(wizardData);
        let wizardPage : ALPageWizardPage = new ALPageWizardPage(this._toolsExtensionContext, settings, wizardData);
        wizardPage.show();
    }

} 