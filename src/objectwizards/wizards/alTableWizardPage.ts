'use strict';

import * as path from 'path';
import { ProjectItemWizardPage } from './projectItemWizardPage';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALTableWizardData } from './alTableWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ALTableWizardFieldData } from './alTableWizardFieldData';
import { ALTableSyntaxBuilder } from '../syntaxbuilders/alTableSyntaxBuilder';

export class ALTableWizardPage extends ProjectItemWizardPage {
    private _tableWizardData : ALTableWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALTableWizardData) {
        super(toolsExtensionContext, "AL Table Wizard", settings);
        this._tableWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._tableWizardData
        });
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'altablewizard', 'altablewizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALTableWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._tableWizardData.objectId = data.objectId;
        this._tableWizardData.objectName = data.objectName;
        this._tableWizardData.fields = this.validateFields(data.fields);
    
        //build new object
        var builder : ALTableSyntaxBuilder = new ALTableSyntaxBuilder();
        var source = builder.buildFromTableWizardData(this._tableWizardData);
        this.createObjectFile('Table', this._tableWizardData.objectId, this._tableWizardData.objectName, source);

        return true;
    }

    protected validateFields(data : any) : ALTableWizardFieldData[] {
        let fields : ALTableWizardFieldData[] = [];

        if ((data) && (data.length > 0)) {
            for (let i = 0; i<data.length; i++) {
                fields.push(new ALTableWizardFieldData(data[i].id, data[i].name, data[i].dataType, data[i].length));
            }
        }


        return fields;
    }

}