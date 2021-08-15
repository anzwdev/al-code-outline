import * as path from 'path';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ToolsGetObjectsListRequest } from '../../langserver/symbolsinformation/toolsGetObjectsListRequest';
import { ToolsSymbolInformationRequest } from '../../langserver/symbolsinformation/toolsSymbolInformationRequest';
import { SymbolWithNameInformation } from '../../symbolsinformation/smbolWithNameInformation';
import { ALPermissionSetSyntaxBuilder } from '../syntaxbuilders/alPermissionSetSyntaxBuilder';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPermissionSetWizardData } from "./alPermissionSetWizardData";
import { ProjectItemWizardPage } from "./projectItemWizardPage";

export class ALPermissionSetWizardPage extends ProjectItemWizardPage {
    private _permissionSetWizardData : ALPermissionSetWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title: string | undefined, settings: ALObjectWizardSettings, data : ALPermissionSetWizardData) {
        if (!title)
            title = "AL Permission Set Wizard";
        super(toolsExtensionContext, title, settings);
        this._permissionSetWizardData = data;
    }

    protected onDocumentLoaded() {
        //send data to the web view
        this.sendMessage({
            command : 'setData',
            data : this._permissionSetWizardData
        });
        this.loadPermissionSets();
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alpermissionsetwizard', 'alpermissionsetwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALPermissionSetWizard";
    }
      
    protected async finishWizard(data : any) : Promise<boolean> {
        await this.setBuilderData(data);
        this.runBuilder();
        return true;
    }

    protected async setBuilderData(data: any) {
        //build parameters
        this._permissionSetWizardData.objectId = data.objectId;
        this._permissionSetWizardData.objectName = data.objectName;
        this._permissionSetWizardData.inclAllObjects = data.inclAllObjects;
        this._permissionSetWizardData.selectedPermissionSetList = data.selectedPermissionSetList;
    
        //load project settings from the language server
        this._permissionSetWizardData.projectSettings = await this.getProjectSettings();

        //get all extension objects
        if (this._permissionSetWizardData.inclAllObjects) {
            let getObjectsRequest = new ToolsGetObjectsListRequest(this._settings.getDestDirectoryPath());
            getObjectsRequest.setIncludeObjectsWithPermissions();
            let getObjectsResponse = await this._toolsExtensionContext.toolsLangServerClient.getObjectsList(getObjectsRequest);
            if ((getObjectsResponse) && (getObjectsResponse.symbols))
                this._permissionSetWizardData.selectedObjectsList = getObjectsResponse.symbols;
        }
    }

    protected runBuilder() {
        //build new object
        var builder : ALPermissionSetSyntaxBuilder = new ALPermissionSetSyntaxBuilder();
        var source = builder.buildFromPermissionSetWizardData(this._settings.getDestDirectoryUri(), this._permissionSetWizardData);
        this.createObjectFile('PermissionSet', '', this._permissionSetWizardData.objectName, source);
    }

    protected async loadPermissionSets() {      
        let response = await this._toolsExtensionContext.toolsLangServerClient.getPermissionSetsList(
            new ToolsSymbolInformationRequest(this._settings.getDestDirectoryPath(), false));
        if (response)
            this._permissionSetWizardData.permissionSetList = SymbolWithNameInformation.toNamesList(response.symbols);

        //let resourceUri = this._settings.getDestDirectoryUri();
        //this._wizardData.codeunitList = await this._toolsExtensionContext.alLangProxy.getCodeunitList(resourceUri);
        if ((this._permissionSetWizardData.permissionSetList) && (this._permissionSetWizardData.permissionSetList.length > 0))
            this.sendMessage({
                command : "setPermissionSets",
                data : this._permissionSetWizardData.permissionSetList
            });
    }

}