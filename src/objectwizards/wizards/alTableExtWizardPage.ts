'use strict';

import * as path from 'path';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ALTableExtSyntaxBuilder } from '../syntaxbuilders/alTableExtSyntaxBuilder';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALTableExtWizardData } from './alTableExtWizardData';
import { WizardTableFieldHelper } from './wizardTableFieldHelper';

export class ALTableExtWizardPage extends ALTableBasedWizardPage {
    private _tableExtWizardData : ALTableExtWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALTableExtWizardData) {
        super(toolsExtensionContext, "AL Table Extension Wizard", settings, data);
        this._tableExtWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'altableextwizard', 'altableextwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALTableExtWizard";
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._tableExtWizardData.objectId = data.objectId;
        this._tableExtWizardData.objectName = data.objectName;
        this._tableExtWizardData.fields = WizardTableFieldHelper.validateFields(data.fields);
        this._tableExtWizardData.selectedTable = data.selectedTable;
    
        await this.finishObjectIdReservation(this._tableExtWizardData);

        //build new object
        var builder : ALTableExtSyntaxBuilder = new ALTableExtSyntaxBuilder();
        var source = builder.buildFromTableExtWizardData(this._settings.getDestDirectoryUri(), this._tableExtWizardData);
        this.createObjectExtensionFile('TableExtension', this._tableExtWizardData.objectId, this._tableExtWizardData.objectName, this._tableExtWizardData.selectedTable, source);

        return true;
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        switch (message.command) {
            case 'loadTypes':
                this.loadTypes();
                return true;
        }
        
        return false;
    }

    protected async loadTypes() {
        let types: string[] = await WizardTableFieldHelper.getAllFieldTypes(this._toolsExtensionContext, this._settings.getDestDirectoryUri());
        // update types
        if (types.length > 0) {
            this.sendMessage({
                command : 'setTypes',
                data : types
            });
        }
    }
}