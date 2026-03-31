import * as path from 'path';
import { ALObjectWizardPage } from './alObjectWizardPage';
import { ALEnumWizardData } from './alEnumWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { ALEnumSyntaxBuilder } from '../syntax_builders/alEnumSyntaxBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';

export class ALEnumWizardPage extends ALObjectWizardPage {
    private _enumWizardData : ALEnumWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALEnumWizardData) {
        super(toolsExtensionContext, "AL Enum Wizard", "azALDevTools.ALEnumWizard", settings, data);
        this._enumWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._enumWizardData
        });
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alenumwizard', 'alenumwizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._enumWizardData.objectId = data.objectId;
        this._enumWizardData.objectName = data.objectName;
        this._enumWizardData.valueList = data.valueList;
        this._enumWizardData.captionList = data.captionList;
        this._enumWizardData.extensible = data.extensible;
    
        await this.finishObjectIdReservation(this._enumWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.EnumType, this._enumWizardData.objectName, this._enumWizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {
            this._enumWizardData.objectNamespace = fileNamespaces.namespace;
            this._enumWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        var builder : ALEnumSyntaxBuilder = new ALEnumSyntaxBuilder();
        var source = builder.buildFromEnumWizardData(this._settings.destDirectoryUri, this._enumWizardData);
        this.createObjectFile(LSObjectKind.EnumType, this._enumWizardData.objectId, this._enumWizardData.objectName, source);

        return true;
    }

} 