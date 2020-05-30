'use strict';

import * as path from 'path';
import { ALTableBasedWizardPage } from "./alTableBasedWizardPage";
import { ALQueryWizardData } from "./alQueryWizardData";
import { ALQuerySyntaxBuilder } from '../syntaxbuilders/alQuerySyntaxBuilder';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';

export class ALQueryWizardPage extends ALTableBasedWizardPage {
    protected _queryWizardData : ALQueryWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALQueryWizardData) {
        super(toolsExtensionContext, "AL Query Wizard", settings, data);
        this._queryWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alquerywizard', 'alquerywizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALQueryWizard";
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

        //build new object
        var builder : ALQuerySyntaxBuilder = new ALQuerySyntaxBuilder();
        var source = builder.buildFromQueryWizardData(this._settings.getDestDirectoryUri(), this._queryWizardData);
        this.createObjectFile('Query', this._queryWizardData.objectId, this._queryWizardData.objectName, source);

        return true;
    }

} 