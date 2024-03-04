'use strict';

import * as path from 'path';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALReportWizardData } from './alReportWizardData';
import { ALReportSyntaxBuilder } from '../syntaxbuilders/alReportSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ToolsSymbolReference } from '../../langserver/symbolsinformation/toolsSymbolReference';

export class ALReportWizardPage extends ALTableBasedWizardPage {
    protected _reportWizardData : ALReportWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALReportWizardData) {
        super(toolsExtensionContext, "AL Report Wizard", settings, data);
        this._reportWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alreportwizard', 'alreportwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALReportWizard";
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
        let referencedObjects: ToolsSymbolReference[] = [];
        if (this._reportWizardData.selectedTable) {
            referencedObjects.push({
                nameWithNamespaceOrId: this._reportWizardData.selectedTable,
                typeName: 'Table'
            });
        }

        let fileNamespaces = await this.getNamespacesInformation('Report', referencedObjects);
        if (fileNamespaces) {
            this._reportWizardData.objectNamespace = fileNamespaces.namespaceName;
            this._reportWizardData.objectUsings = fileNamespaces.usings;
        }

        //load project settings from the language server
        this._reportWizardData.projectSettings = await this.getProjectSettings();

        //build new object
        var builder : ALReportSyntaxBuilder = new ALReportSyntaxBuilder();
        var source = builder.buildFromReportWizardData(this._settings.getDestDirectoryUri(), this._reportWizardData);
        this.createObjectFile('Report', this._reportWizardData.objectId, this._reportWizardData.objectName, source);
       
        return true;
    }

}
