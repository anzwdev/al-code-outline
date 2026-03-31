import * as path from 'path';
import { ALTableBasedWizardPage } from "./alTableBasedWizardPage";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALCodeunitWizardData } from "./alCodeunitWizardData";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { ALCodeunitSyntaxBuilder } from '../syntax_builders/alCodeunitSyntaxBuilder';

export class ALCodeunitWizardPage extends ALTableBasedWizardPage {
    protected _codeunitWizardData : ALCodeunitWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALCodeunitWizardData) {
        super(toolsExtensionContext, "AL Codeunit Wizard", "azALDevTools.ALCodeunitWizard", settings, data);
        this._codeunitWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alcodeunitwizard', 'alcodeunitwizard.html');
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message)) {
            return true;
        }

        switch (message.command) {
            case 'loadInterfaces':
                this.loadInterfaces();
                return true;
        }
        
        return false;
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._codeunitWizardData.objectId = data.objectId;
        this._codeunitWizardData.objectName = data.objectName;
        this._codeunitWizardData.selectedTable = data.selectedTable;
        this._codeunitWizardData.interface = data.interface;
        
        await this.finishObjectIdReservation(this._codeunitWizardData);

        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];
        if (this._codeunitWizardData.selectedTable) {
            referencedObjects.push(this._codeunitWizardData.selectedTable);
        }
        if (this._codeunitWizardData.interface) {
            referencedObjects.push(this._codeunitWizardData.interface);
        }

        let namespaceInformation = await this.getNamespaceAndUsings(LSObjectKind.Codeunit, this._codeunitWizardData.objectName, 
            this._codeunitWizardData.objectNamespace, referencedObjects);
        if (namespaceInformation) {
            this._codeunitWizardData.objectNamespace = namespaceInformation.namespace;
            this._codeunitWizardData.objectUsings = namespaceInformation.usings;
        }

        //build new object
        let builder : ALCodeunitSyntaxBuilder = new ALCodeunitSyntaxBuilder(this.context);
        let source = await builder.buildFromCodeunitWizardDataAsync(this._settings.destDirectoryUri, this._codeunitWizardData);
        this.createObjectFile(LSObjectKind.Codeunit, this._codeunitWizardData.objectId, this._codeunitWizardData.objectName, source);

        return true;
    }

    protected async loadInterfaces() {
        this._codeunitWizardData.interfaceList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.Interface);

        //this._codeunitWizardData.interfaceList = await this._toolsExtensionContext.alLangProxy.getInterfaceList(resourceUri);
        if ((this._codeunitWizardData.interfaceList) && (this._codeunitWizardData.interfaceList.length > 0)) {
            this.sendMessage({
                command : "setInterfaces",
                data : this._codeunitWizardData.interfaceList
            });
        }
    }

}