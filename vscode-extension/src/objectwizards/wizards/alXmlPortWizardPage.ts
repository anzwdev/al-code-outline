'use strict';

import * as path from 'path';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALXmlPortWizardData } from './alXmlPortWizardData';
import { ALXmlPortSyntaxBuilder } from '../syntaxbuilders/alXmlPortSyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ToolsSymbolReference } from '../../langserver/symbolsinformation/toolsSymbolReference';

export class ALXmlPortWizardPage extends ALTableBasedWizardPage {
    protected _xmlPortWizardData : ALXmlPortWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALXmlPortWizardData) {
        super(toolsExtensionContext, "AL XmlPort Wizard", settings, data);
        this._xmlPortWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alxmlportwizard', 'alxmlportwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALXmlPortWizard";
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

        //get namespaces information
        let referencedObjects: ToolsSymbolReference[] = [];
        if (this._xmlPortWizardData.selectedTable) {
            referencedObjects.push({
                nameWithNamespaceOrId: this._xmlPortWizardData.selectedTable,
                typeName: 'Table'
            });
        }

        let fileNamespaces = await this.getNamespacesInformation('XmlPort', referencedObjects);
        if (fileNamespaces) {
            this._xmlPortWizardData.objectNamespace = fileNamespaces.namespaceName;
            this._xmlPortWizardData.objectUsings = fileNamespaces.usings;
        }

        //load project settings from the language server
        this._xmlPortWizardData.projectSettings = await this.getProjectSettings();        

        //build new object
        var builder : ALXmlPortSyntaxBuilder = new ALXmlPortSyntaxBuilder();
        var source = builder.buildFromXmlPortWizardData(this._settings.getDestDirectoryUri(), this._xmlPortWizardData);
        this.createObjectFile('XmlPort', this._xmlPortWizardData.objectId, this._xmlPortWizardData.objectName, source);

        return true;
    }

}
