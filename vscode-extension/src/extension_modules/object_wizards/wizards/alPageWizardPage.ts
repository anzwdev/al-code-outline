import * as path from 'path';
import { ALPageWizardData } from './alPageWizardData';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALPageWizardFastTabData } from './alPageWizardFastTabData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { DevToolsExtensionSettings } from '../../extension_settings/devToolsExtensionSettings';
import { ALFieldToolTipsLocation } from '../../extension_settings/alFieldToolTipsLocation';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { ALPageSyntaxBuilder } from '../syntax_builders/alPageSyntaxBuilder';

export class ALPageWizardPage extends ALTableBasedWizardPage {
    protected _pageWizardData : ALPageWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALPageWizardData) {
        super(toolsExtensionContext, "AL Page Wizard", "azALDevTools.ALPageWizard", settings, data);
        this._pageWizardData = data;
        this._includeToolTips = data.reuseToolTips;
        this._toolTipsSourceDependencies = data.toolTipsSource;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alpagewizard', 'alpagewizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        let extensionSettings = new DevToolsExtensionSettings(this._settings.destDirectoryUri);
        let fieldToolTipsLocation = extensionSettings.getFieldToolTipsLocation(this._settings.projectProfile?.platformCapabilities);

        //build parameters
        this._pageWizardData.objectId = data.objectId;
        this._pageWizardData.objectName = data.objectName;
        this._pageWizardData.selectedTable = data.selectedTable;
        this._pageWizardData.pageType = data.pageType;
        this._pageWizardData.fastTabs = data.fastTabs;
        this._pageWizardData.applicationArea = data.applicationArea;
        this._pageWizardData.applicationAreaMode = extensionSettings.getAppAreaMode(this._settings.projectProfile?.platformCapabilities);
        this._pageWizardData.usageCategory = data.usageCategory;
        this._pageWizardData.caption = data.caption;
        this._pageWizardData.apiPublisher = data.apiPublisher;
        this._pageWizardData.apiGroup = data.apiGroup;
        this._pageWizardData.apiVersion = data.apiVersion;
        this._pageWizardData.entityName = data.entityName;
        this._pageWizardData.entitySetName = data.entitySetName;
        this._pageWizardData.createTooltips = (!!data.createTooltips) && (fieldToolTipsLocation === ALFieldToolTipsLocation.page);
    
        //information about selected fields
        this._pageWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._pageWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        this._pageWizardData.selectedFlowFilterList = [];
        if (data.flowFilters) {
            for (var i = 0; i<data.flowFilters.length; i++) {
                this._pageWizardData.selectedFlowFilterList.push(data.flowFilters[i]);
            }
        }

        //information about fast tabs
        this._pageWizardData.fastTabsData = [];
        if (data.fastTabsData) {
            for (var i = 0; i<data.fastTabsData.length; i++) {
                var sourceFastTabDetails = data.fastTabsData[i];
                var fastTabDetails = new ALPageWizardFastTabData(sourceFastTabDetails.name);
                if (sourceFastTabDetails.fields) {
                    for (var fld = 0; fld < sourceFastTabDetails.fields.length; fld++) {
                        fastTabDetails.fields.push(sourceFastTabDetails.fields[fld]);
                    }
                }
                this._pageWizardData.fastTabsData.push(fastTabDetails);
            }
        }

        await this.finishObjectIdReservation(this._pageWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._pageWizardData.selectedTable) {
            referencedObjects.push(this._pageWizardData.selectedTable);
        }

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.Page, this._pageWizardData.objectName, this._pageWizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {
            this._pageWizardData.objectNamespace = fileNamespaces.namespace;
            this._pageWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        let builder : ALPageSyntaxBuilder = new ALPageSyntaxBuilder();
        let source = builder.buildFromPageWizardData(this._settings.destDirectoryUri, this._pageWizardData, this._settings.projectProfile);
        this.createObjectFile(LSObjectKind.Page, this._pageWizardData.objectId, this._pageWizardData.objectName, source);

        return true;
    }

}
