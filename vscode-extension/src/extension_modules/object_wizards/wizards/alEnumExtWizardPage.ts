import * as path from 'path';
import { ALObjectWizardPage } from './alObjectWizardPage';
import { ALEnumExtWizardData } from './alEnumExtWizardData';
import { ALObjectWizardSettings } from './alObjectWizardSettings';
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { ALEnumExtSyntaxBuilder } from '../syntax_builders/alEnumExtSyntaxBuilder';

export class ALEnumExtWizardPage extends ALObjectWizardPage {
    private _enumExtWizardData : ALEnumExtWizardData;
    
    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALEnumExtWizardData) {
        super(toolsExtensionContext, "AL Enum Extension Wizard", "azALDevTools.ALEnumExtWizard", settings, data);
        this._enumExtWizardData = data;
    }

    //initialize wizard
    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._enumExtWizardData
        });

        //load base enums
        if ((!this._enumExtWizardData.baseEnumList) || (this._enumExtWizardData.baseEnumList.length === 0)) {
            this.loadBaseEnums();
        }
    }

    protected async loadBaseEnums() {
        this._enumExtWizardData.baseEnumList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.EnumType);

        //this._enumExtWizardData.baseEnumList = await this._toolsExtensionContext.alLangProxy.getEnumList(this._settings.getDestDirectoryUri());
        this.sendMessage({
            command : "setEnums",
            data : this._enumExtWizardData.baseEnumList
        });
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alenumextwizard', 'alenumextwizard.html');
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._enumExtWizardData.objectId = data.objectId;
        this._enumExtWizardData.objectName = data.objectName;
        this._enumExtWizardData.baseEnum = data.baseEnum;
        this._enumExtWizardData.valueList = data.valueList;
        this._enumExtWizardData.captionList = data.captionList;

        let firstValueId = Number.parseInt(data.firstValueId);
        if (Number.isNaN(firstValueId)) {
            this._enumExtWizardData.firstValueId = 0;
        } else {
            this._enumExtWizardData.firstValueId = firstValueId;
        }

        await this.finishObjectIdReservation(this._enumExtWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._enumExtWizardData.baseEnum) {
            referencedObjects.push(this._enumExtWizardData.baseEnum);
        }

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.EnumExtensionType, this._enumExtWizardData.objectName, this._enumExtWizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {
            this._enumExtWizardData.objectNamespace = fileNamespaces.namespace;
            this._enumExtWizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        let builder : ALEnumExtSyntaxBuilder = new ALEnumExtSyntaxBuilder();
        let source = builder.buildFromEnumExtWizardData(this._settings.destDirectoryUri, this._enumExtWizardData);
        this.createObjectExtensionFile(LSObjectKind.EnumExtensionType, this._enumExtWizardData.objectId, this._enumExtWizardData.objectName,
            this._enumExtWizardData.baseEnum?.name ?? "", source);

        return true;
    }

} 