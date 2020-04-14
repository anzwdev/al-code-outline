import * as path from 'path';
import { ALInterfaceWizardData } from "./alInterfaceWizardData";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ProjectItemWizardPage } from "./projectItemWizardPage";
import { ALInterfaceSyntaxBuilder } from '../syntaxbuilders/alInterfaceSyntaxBuilder';

export class ALInterfaceWizardPage extends ProjectItemWizardPage {
    protected _wizardData : ALInterfaceWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALInterfaceWizardData) {
        super(toolsExtensionContext, "AL Interface Wizard", settings);
        this._wizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alinterfacewizard', 'alinterfacewizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALInterfaceWizard";
    }

    protected onDocumentLoaded() {
        super.onDocumentLoaded();
        this.loadCodeunits();
    }

    protected async finishWizard(data : any) : Promise<boolean> {
        //build parameters
        this._wizardData.objectName = data.objectName;
        this._wizardData.baseCodeunitName = data.baseCodeunitName;
        
        //build new object
        let builder : ALInterfaceSyntaxBuilder = new ALInterfaceSyntaxBuilder(this._toolsExtensionContext);
        let source = await builder.buildFromInterfaceWizardDataAsync(this._settings.getDestDirectoryUri(),
            this._wizardData);
        this.createObjectFile('Interface', '', this._wizardData.objectName, source);

        return true;
    }

    protected async loadCodeunits() {
        let resourceUri = this._settings.getDestDirectoryUri();
        this._wizardData.codeunitList = await this._toolsExtensionContext.alLangProxy.getCodeunitList(resourceUri);
        if ((this._wizardData.codeunitList) && (this._wizardData.codeunitList.length > 0))
            this.sendMessage({
                command : "setCodeunits",
                data : this._wizardData.codeunitList
            });
    }

}