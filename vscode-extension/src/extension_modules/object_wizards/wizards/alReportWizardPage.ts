import * as path from 'path';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALReportWizardData } from './alReportWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALReportSyntaxBuilder } from '../syntax_builders/alReportSyntaxBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';

export class ALReportWizardPage extends ALTableBasedWizardPage {
    protected _reportWizardData : ALReportWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALReportWizardData) {
        super(toolsExtensionContext, "AL Report Wizard", "azALDevTools.ALReportWizard", settings, data);
        this._reportWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alreportwizard', 'alreportwizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._reportWizardData.objectId = data.objectId;
        this._reportWizardData.objectName = data.objectName;
        this._reportWizardData.selectedTable = data.selectedTable;
        this._reportWizardData.applicationArea = data.applicationArea;
        this._reportWizardData.usageCategory = data.usageCategory;
        this._reportWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._reportWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        await this.finishObjectIdReservation(this._reportWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._reportWizardData.selectedTable) {
            referencedObjects.push(this._reportWizardData.selectedTable);
        }

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.Report, 
            this._reportWizardData.objectName, this._reportWizardData.objectNamespace, referencedObjects);

        if (fileNamespaces) {
            this._reportWizardData.objectNamespace = fileNamespaces.namespace;
            this._reportWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        var builder : ALReportSyntaxBuilder = new ALReportSyntaxBuilder();
        var source = builder.buildFromReportWizardData(this._settings.destDirectoryUri, this._reportWizardData, this._settings.projectProfile);
        this.createObjectFile(LSObjectKind.Report, this._reportWizardData.objectId, this._reportWizardData.objectName, source);
       
        return true;
    }

}
