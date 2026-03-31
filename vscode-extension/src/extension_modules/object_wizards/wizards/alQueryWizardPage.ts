import * as path from 'path';
import { ALTableBasedWizardPage } from "./alTableBasedWizardPage";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { ALQuerySyntaxBuilder } from '../syntax_builders/alQuerySyntaxBuilder';

export class ALQueryWizardPage extends ALTableBasedWizardPage {
    protected _queryWizardData : ALQueryWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALQueryWizardData) {
        super(toolsExtensionContext, "AL Query Wizard", "azALDevTools.ALQueryWizard", settings, data);
        this._queryWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alquerywizard', 'alquerywizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._queryWizardData.objectId = data.objectId;
        this._queryWizardData.objectName = data.objectName;
        this._queryWizardData.selectedTable = data.selectedTable;
        this._queryWizardData.queryType = data.queryType;
        this._queryWizardData.apiPublisher = data.apiPublisher;
        this._queryWizardData.apiGroup = data.apiGroup;
        this._queryWizardData.apiVersion = data.apiVersion;
        this._queryWizardData.entityName = data.entityName;
        this._queryWizardData.entitySetName = data.entitySetName;

        this._queryWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._queryWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        await this.finishObjectIdReservation(this._queryWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._queryWizardData.selectedTable) {
            referencedObjects.push(this._queryWizardData.selectedTable);
        }

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.Query, this._queryWizardData.objectName, this._queryWizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {
            this._queryWizardData.objectNamespace = fileNamespaces.namespace;
            this._queryWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        var builder : ALQuerySyntaxBuilder = new ALQuerySyntaxBuilder();
        var source = builder.buildFromQueryWizardData(this._settings.destDirectoryUri, this._queryWizardData, this._settings.projectProfile);
        this.createObjectFile(LSObjectKind.Query, this._queryWizardData.objectId, this._queryWizardData.objectName, source);

        return true;
    }

} 