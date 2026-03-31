import * as path from 'path';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { ALTableBasedWizardPage } from './alTableBasedWizardPage';
import { ALTableExtWizardData } from './alTableExtWizardData';
import { WizardTableFieldHelper } from './wizardTableFieldHelper';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALTableExtSyntaxBuilder } from '../syntax_builders/alTableExtSyntaxBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSPIObjectListItemHelper } from '../../../langserver/project_information/symbols/lspiObjectListItemHelper';

export class ALTableExtWizardPage extends ALTableBasedWizardPage {
    private _tableExtWizardData : ALTableExtWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALTableExtWizardData) {
        super(toolsExtensionContext, "AL Table Extension Wizard", "azALDevTools.ALTableExtWizard", settings, data);
        this._tableExtWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'altableextwizard', 'altableextwizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._tableExtWizardData.objectId = data.objectId;
        this._tableExtWizardData.objectName = data.objectName;
        this._tableExtWizardData.fields = WizardTableFieldHelper.validateFields(data.fields);
        this._tableExtWizardData.selectedTable = LSPIObjectListItemHelper.toObjectIdentifierOrUndefined(data.selectedTable);
    
        await this.finishObjectIdReservation(this._tableExtWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._tableExtWizardData.selectedTable) {
            referencedObjects.push(this._tableExtWizardData.selectedTable);
        }
        let namespaceInformation = await this.getNamespaceAndUsings(LSObjectKind.TableExtension, this._tableExtWizardData.objectName, 
            this._tableExtWizardData.objectNamespace, referencedObjects);

        if (namespaceInformation) {
            this._tableExtWizardData.objectNamespace = namespaceInformation.namespace;
            this._tableExtWizardData.objectUsings = namespaceInformation.usings;
        }

        //build new object
        var builder : ALTableExtSyntaxBuilder = new ALTableExtSyntaxBuilder();
        var source = builder.buildFromTableExtWizardData(this._settings.destDirectoryUri, this._tableExtWizardData);
        this.createObjectExtensionFile(LSObjectKind.TableExtension, this._tableExtWizardData.objectId, this._tableExtWizardData.objectName, this._tableExtWizardData.selectedTable?.name ?? "", source);

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