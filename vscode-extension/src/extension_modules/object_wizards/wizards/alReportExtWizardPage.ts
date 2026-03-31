import * as path from 'path';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALReportExtWizardData } from './alReportExtWizardData';
import { ALObjectWizardPage } from './alObjectWizardPage';
import { ALReportExtWizardDataItemData } from './alReportExtWizardDataItemData';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { ALReportExtSyntaxBuilder } from '../syntax_builders/alReportExtSyntaxBuilder';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';

export class ALReportExtWizardPage extends ALObjectWizardPage {
    protected _reportExtWizardData : ALReportExtWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALReportExtWizardData) {
        super(toolsExtensionContext, "AL Report Extension Wizard", "azALDevTools.ALReportExtWizard", settings, data);
        this._reportExtWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._reportExtWizardData
        });
        this.loadReports();
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alreportextwizard', 'alreportextwizard.html');
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        switch (message.command) {
            case 'loadReports':
                this.loadReports();
                return true;
            case 'selectReport':
                this.loadBaseReport(message.baseReport);
                return true;
        }

        return false;
    }

    protected anyToDataItem(data: any): ALReportExtWizardDataItemData {
        let dataItem: ALReportExtWizardDataItemData = new ALReportExtWizardDataItemData(data.name);
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                dataItem.fields.push(data.fields[i]);
            }
        }            
        return dataItem;
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._reportExtWizardData.objectId = data.objectId;
        this._reportExtWizardData.objectName = data.objectName;
        this._reportExtWizardData.baseReport = data.baseReport;
        this._reportExtWizardData.dataItems = [];

        if ((data.dataItems) && (data.dataItems.length > 0)) {
            for (let i=0; i<data.dataItems.length; i++) {
                this._reportExtWizardData.dataItems.push(this.anyToDataItem(data.dataItems[i]));
            }
        }

        await this.finishObjectIdReservation(this._reportExtWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._reportExtWizardData.baseReport) {
            referencedObjects.push(this._reportExtWizardData.baseReport);
        }

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.ReportExtension, 
            this._reportExtWizardData.objectName, this._reportExtWizardData.objectNamespace,
            referencedObjects);

        if (fileNamespaces) {
            this._reportExtWizardData.objectNamespace = fileNamespaces.namespace;
            this._reportExtWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        let builder : ALReportExtSyntaxBuilder = new ALReportExtSyntaxBuilder();
        let source = await builder.buildFromReportExtWizardData(this._settings.destDirectoryUri,
            this._reportExtWizardData);
        this.createObjectExtensionFile(LSObjectKind.ReportExtension, this._reportExtWizardData.objectId, this._reportExtWizardData.objectName, this._reportExtWizardData.baseReport?.name ?? "", source);

        return true;
    }

    protected async loadReports() {
        this._reportExtWizardData.reportList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.Report);
        if ((this._reportExtWizardData.reportList) && (this._reportExtWizardData.reportList.length > 0)) {
            this.sendMessage({
                command : "setReports",
                data : this._reportExtWizardData.reportList
            });
        }
    }

    protected async loadBaseReport(baseReport: string | undefined) {
        //!!! TO-DO
        /*
        if (!baseReport)
            return;

        let reportReference: ToolsSymbolReference = {
            name: baseReport
        };

        let response = await this._toolsExtensionContext.toolsLangServerClient.getReportDetails(
            new ToolsGetReportDetailsRequest(this._settings.getDestDirectoryPath(), reportReference, true, true));

        if ((response) && (response.symbol)) {
            this.sendMessage({
                command: 'setBaseReport',
                data: response.symbol
            });
        }
        */
    }

}
