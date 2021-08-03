import * as path from 'path';
import { ALTableBasedWizardPage } from "./alTableBasedWizardPage";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALCodeunitWizardData } from "./alCodeunitWizardData";
import { ALCodeunitSyntaxBuilder } from '../syntaxbuilders/alCodeunitSyntaxBuilder';
import { ToolsSymbolInformationRequest } from '../../langserver/symbolsinformation/toolsSymbolInformationRequest';
import { SymbolWithNameInformation } from '../../symbolsinformation/smbolWithNameInformation';

export class ALCodeunitWizardPage extends ALTableBasedWizardPage {
    protected _codeunitWizardData : ALCodeunitWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALCodeunitWizardData) {
        super(toolsExtensionContext, "AL Codeunit Wizard", settings, data);
        this._codeunitWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alcodeunitwizard', 'alcodeunitwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALCodeunitWizard";
    }

    protected processWebViewMessage(message : any) : boolean {
        if (super.processWebViewMessage(message))
            return true;

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
        this._codeunitWizardData.interfaceName = data.interfaceName;
        
        //build new object
        let builder : ALCodeunitSyntaxBuilder = new ALCodeunitSyntaxBuilder(this._toolsExtensionContext);
        let source = await builder.buildFromCodeunitWizardDataAsync(this._settings.getDestDirectoryUri(),
            this._codeunitWizardData);
        this.createObjectFile('Codeunit', this._codeunitWizardData.objectId, this._codeunitWizardData.objectName, source);

        return true;
    }

    protected async loadInterfaces() {
        let resourceUri = this._settings.getDestDirectoryUri();
        if (this._toolsExtensionContext.alLangProxy.supportsInterfaces(resourceUri)) {
            let response = await this._toolsExtensionContext.toolsLangServerClient.getInterfacesList(
                new ToolsSymbolInformationRequest(this._settings.getDestDirectoryPath()));
            if (response)
            this._codeunitWizardData.interfaceList = SymbolWithNameInformation.toNamesList(response.symbols);
            //this._codeunitWizardData.interfaceList = await this._toolsExtensionContext.alLangProxy.getInterfaceList(resourceUri);
            if ((this._codeunitWizardData.interfaceList) && (this._codeunitWizardData.interfaceList.length > 0))
                this.sendMessage({
                    command : "setInterfaces",
                    data : this._codeunitWizardData.interfaceList
                });
        }
    }

}