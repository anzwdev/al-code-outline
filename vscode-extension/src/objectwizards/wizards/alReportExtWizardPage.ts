import * as path from 'path';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALReportExtWizardData } from './alReportExtWizardData';
import { ALReportExtSyntaxBuilder } from '../syntaxbuilders/alReportExtSyntaxBuilder';
import { ProjectItemWizardPage } from './projectItemWizardPage';
import { ToolsSymbolInformationRequest } from '../../langserver/symbolsinformation/toolsSymbolInformationRequest';
import { SymbolWithNameInformation } from '../../symbolsinformation/smbolWithNameInformation';
import { ToolsGetReportDetailsRequest } from '../../langserver/symbolsinformation/toolsGetReportDetailsRequest';
import { ALReportExtWizardDataItemData } from './alReportExtWizardDataItemData';

export class ALReportExtWizardPage extends ProjectItemWizardPage {
    protected _reportExtWizardData : ALReportExtWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALReportExtWizardData) {
        super(toolsExtensionContext, "AL Report Extension Wizard", settings, data);
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

    protected getViewType() : string {
        return "azALDevTools.ALReportExtWizard";
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

        //build new object
        let builder : ALReportExtSyntaxBuilder = new ALReportExtSyntaxBuilder();
        let source = await builder.buildFromReportExtWizardData(this._settings.getDestDirectoryUri(),
            this._reportExtWizardData);
        this.createObjectExtensionFile('ReportExtension', this._reportExtWizardData.objectId, this._reportExtWizardData.objectName, this._reportExtWizardData.baseReport, source);

        return true;
    }

    protected async loadReports() {
        let response = await this._toolsExtensionContext.toolsLangServerClient.getReportsList(
            new ToolsSymbolInformationRequest(this._settings.getDestDirectoryPath(), false));
        if (response)
            this._reportExtWizardData.reportList = SymbolWithNameInformation.toNamesList(response.symbols);

        if ((this._reportExtWizardData.reportList) && (this._reportExtWizardData.reportList.length > 0)) {
            this.sendMessage({
                command : "setReports",
                data : this._reportExtWizardData.reportList
            });
        }
    }

    protected async loadBaseReport(baseReport: string | undefined) {
        if (!baseReport)
            return;

        let response = await this._toolsExtensionContext.toolsLangServerClient.getReportDetails(
            new ToolsGetReportDetailsRequest(this._settings.getDestDirectoryPath(), baseReport, true, true));

        if ((response) && (response.symbol)) {
            this.sendMessage({
                command: 'setBaseReport',
                data: response.symbol
            });
        }
    }

}
