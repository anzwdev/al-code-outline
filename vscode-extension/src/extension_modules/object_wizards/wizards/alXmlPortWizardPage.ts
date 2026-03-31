'use strict';

import * as path from 'path';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALXmlPortWizardData } from './alXmlPortWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALXmlPortSyntaxBuilder } from '../syntax_builders/alXmlPortSyntaxBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';

export class ALXmlPortWizardPage extends ALTableBasedWizardPage {
    protected _xmlPortWizardData : ALXmlPortWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALXmlPortWizardData) {
        super(toolsExtensionContext, "AL XmlPort Wizard", "azALDevTools.ALXmlPortWizard", settings, data);
        this._xmlPortWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alxmlportwizard', 'alxmlportwizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._xmlPortWizardData.objectId = data.objectId;
        this._xmlPortWizardData.objectName = data.objectName;
        this._xmlPortWizardData.selectedTable = data.selectedTable;
        this._xmlPortWizardData.fieldNodeType = data.fieldNodeType;
        this._xmlPortWizardData.selectedFieldList = [];
        if (data.fields) {
            for (var i = 0; i<data.fields.length; i++) {
                this._xmlPortWizardData.selectedFieldList.push(data.fields[i]);
            }
        }

        await this.finishObjectIdReservation(this._xmlPortWizardData);

        let destDirectoryUri = this._settings.destDirectoryUri;

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._xmlPortWizardData.selectedTable) {
            referencedObjects.push(this._xmlPortWizardData.selectedTable);
        }
        let namespaceInformation = await this.getNamespaceAndUsings(LSObjectKind.XmlPort, this._xmlPortWizardData.objectName, 
            this._xmlPortWizardData.objectNamespace, referencedObjects);

        if (namespaceInformation) {
            this._xmlPortWizardData.objectNamespace = namespaceInformation.namespace;
            this._xmlPortWizardData.objectUsings = namespaceInformation.usings;
        }

        //build new object
        var builder : ALXmlPortSyntaxBuilder = new ALXmlPortSyntaxBuilder();
        var source = builder.buildFromXmlPortWizardData(destDirectoryUri, this._xmlPortWizardData, this._settings.projectProfile);
        this.createObjectFile(LSObjectKind.XmlPort, this._xmlPortWizardData.objectId, this._xmlPortWizardData.objectName, source);

        return true;
    }

}
