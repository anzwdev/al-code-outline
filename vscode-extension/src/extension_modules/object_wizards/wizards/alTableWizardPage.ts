import * as path from 'path';
import { ALObjectWizardPage } from './alObjectWizardPage';
import { ALTableWizardData } from './alTableWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { WizardTableFieldHelper } from './wizardTableFieldHelper';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALTableSyntaxBuilder } from '../syntax_builders/alTableSyntaxBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';

export class ALTableWizardPage extends ALObjectWizardPage {
    private _tableWizardData : ALTableWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALTableWizardData) {
        super(toolsExtensionContext, "AL Table Wizard", "azALDevTools.ALTableWizard", settings, data);
        this._tableWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._tableWizardData
        });
        //load enums
        this.loadTypes();
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'altablewizard', 'altablewizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._tableWizardData.objectId = data.objectId;
        this._tableWizardData.objectName = data.objectName;
        this._tableWizardData.dataClassification = data.dataClassification;
        this._tableWizardData.dataPerCompany = !!data.dataPerCompany;
        this._tableWizardData.fields = WizardTableFieldHelper.validateFields(data.fields);
    
        await this.finishObjectIdReservation(this._tableWizardData);

        //build new object
        var builder : ALTableSyntaxBuilder = new ALTableSyntaxBuilder();
        var source = builder.buildFromTableWizardData(this._settings.destDirectoryUri, this._tableWizardData, this._settings.projectProfile);
        this.createObjectFile(LSObjectKind.Table, this._tableWizardData.objectId, this._tableWizardData.objectName, source);

        return true;
    }

    protected async loadTypes() {
        let types: string[] = await WizardTableFieldHelper.getAllFieldTypes(this.context, this._settings.destDirectoryUri);
        // update types
        if (types.length > 0) {
            this.sendMessage({
                command : 'setTypes',
                data : types
            });
        }
    }
}