import * as path from 'path';
import { ALInterfaceWizardData } from "./alInterfaceWizardData";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALObjectWizardPage } from "./alObjectWizardPage";
import { DevToolsExtensionContext } from '../../../devToolsExtensionContext';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { ALInterfaceSyntaxBuilder } from '../syntax_builders/alInterfaceSyntaxBuilder';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';

export class ALInterfaceWizardPage extends ALObjectWizardPage {
    protected _wizardData : ALInterfaceWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALInterfaceWizardData) {
        super(toolsExtensionContext, "AL Interface Wizard", "azALDevTools.ALInterfaceWizard", settings, data);
        this._wizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alinterfacewizard', 'alinterfacewizard.html');
    }

    protected onDocumentLoaded() {
        super.onDocumentLoaded();
        this.loadCodeunits();
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._wizardData.objectName = data.objectName;
        this._wizardData.baseCodeunit = data.baseCodeunit;
        
        //get namespaces information
        let referencedObjects: LSPIObjectIdentifier[] = [];

        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.Interface, this._wizardData.objectName, this._wizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {
            this._wizardData.objectNamespace = fileNamespaces.namespace;
            this._wizardData.objectUsings = fileNamespaces.usings;
        }

        //build new object
        let builder : ALInterfaceSyntaxBuilder = new ALInterfaceSyntaxBuilder(this.context);
        let source = await builder.buildFromInterfaceWizardDataAsync(this._settings.destDirectoryUri,
            this._wizardData);
        this.createObjectFile(LSObjectKind.Interface, 0, this._wizardData.objectName, source);

        return true;
    }

    protected async loadCodeunits() {
        this._wizardData.codeunitList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.Codeunit);

        if ((this._wizardData.codeunitList) && (this._wizardData.codeunitList.length > 0)) {
            this.sendMessage({
                command : "setCodeunits",
                data : this._wizardData.codeunitList
            });
        }
    }

}