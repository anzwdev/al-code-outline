import * as path from 'path';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ToolsGetObjectsListRequest } from '../../langserver/symbolsinformation/toolsGetObjectsListRequest';
import { ToolsSymbolInformationRequest } from '../../langserver/symbolsinformation/toolsSymbolInformationRequest';
import { SymbolWithNameInformation } from '../../symbolsinformation/smbolWithNameInformation';
import { ALPermissionSetSyntaxBuilder } from '../syntaxbuilders/alPermissionSetSyntaxBuilder';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPermissionSetWizardData } from "./alPermissionSetWizardData";
import { ProjectItemWizardPage } from "./projectItemWizardPage";
import { ObjectInformation } from '../../symbolsinformation/objectInformation';
import { ToolsSymbolReference } from '../../langserver/symbolsinformation/toolsSymbolReference';

export class ALPermissionSetWizardPage extends ProjectItemWizardPage {
    private _permissionSetWizardData : ALPermissionSetWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title: string | undefined, settings: ALObjectWizardSettings, data : ALPermissionSetWizardData) {
        if (!title)
            title = "AL Permission Set Wizard";
        super(toolsExtensionContext, title, settings, data);
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

    protected getWizardObjectType(): string {
        return 'PermissionSet';
    }

    protected async setBuilderData(data: any) {
        //build parameters
        this._permissionSetWizardData.objectId = data.objectId;
        this._permissionSetWizardData.objectName = data.objectName;
        this._permissionSetWizardData.objectCaption = data.objectCaption;
        this._permissionSetWizardData.inclAllObjects = data.inclAllObjects;
        this._permissionSetWizardData.selectedPermissionSetList = data.selectedPermissionSetList;
    
        //load project settings from the language server
        this._permissionSetWizardData.projectSettings = await this.getProjectSettings();

        await this.finishObjectIdReservation(this._permissionSetWizardData);

        //get all extension objects
        if (this._permissionSetWizardData.inclAllObjects) {
            let getObjectsRequest = new ToolsGetObjectsListRequest(this._settings.getDestDirectoryPath());
            getObjectsRequest.setIncludeObjectsWithPermissions();
            let getObjectsResponse = await this._toolsExtensionContext.toolsLangServerClient.getObjectsList(getObjectsRequest);
            if ((getObjectsResponse) && (getObjectsResponse.symbols)) {
                this._permissionSetWizardData.selectedObjectsList = this.removeObjectsWithFullInherentPermissions(getObjectsResponse.symbols);
            }
        }

        //get namespaces information
        let referencedObjects = this.collectReferencedObjects();
        let fileNamespaces = await this.getNamespacesInformation(this.getWizardObjectType(), referencedObjects);
        if (fileNamespaces) {            
            this._permissionSetWizardData.objectNamespace = fileNamespaces.namespaceName;
            this._permissionSetWizardData.objectUsings = fileNamespaces.usings;
            this.addReferencedObjectsNamespaces();
        }
    }

    protected collectReferencedObjects(): ToolsSymbolReference[] {
        let referencedObjects: ToolsSymbolReference[] = [];
        if (this._permissionSetWizardData.selectedPermissionSetList) {
            for (let i=0; i<this._permissionSetWizardData.selectedPermissionSetList.length; i++) {
                referencedObjects.push({
                    nameWithNamespaceOrId: this._permissionSetWizardData.selectedPermissionSetList[i],
                    typeName: 'PermissionSet'
                });
            }
        }
        return referencedObjects;
    }

    protected addReferencedObjectsNamespaces() {
        //collect unique namespaces from namespace property of elements of this._permissionSetWizardData.selectedObjectsList array
        if ((this._permissionSetWizardData.objectNamespace) && (this._permissionSetWizardData.objectNamespace !== "") && (this._permissionSetWizardData.selectedObjectsList)) {
            if (!this._permissionSetWizardData.objectUsings) {
                this._permissionSetWizardData.objectUsings = [];
            }

            for (let i=0; i<this._permissionSetWizardData.selectedObjectsList.length; i++) {
                let namespaceName = this._permissionSetWizardData.selectedObjectsList[i].namespace;
                if ((namespaceName) && (namespaceName !== this._permissionSetWizardData.objectNamespace) && (this._permissionSetWizardData.objectUsings.indexOf(namespaceName) < 0)) {
                    this._permissionSetWizardData.objectUsings.push(namespaceName);
                }
            }
        }
    }

    protected removeObjectsWithFullInherentPermissions(allObjects: ObjectInformation[]): ObjectInformation[] {
        let objects : ObjectInformation[] = [];
        for (let i=0; i<allObjects.length; i++) {
            if (!allObjects[i].fullInherentPermissions) {
                objects.push(allObjects[i]);
            }
        }
        return objects;
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