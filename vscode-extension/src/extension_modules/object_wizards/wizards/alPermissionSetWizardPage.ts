import * as path from 'path';
import { DevToolsExtensionContext } from "../../../devToolsExtensionContext";
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPermissionSetWizardData } from "./alPermissionSetWizardData";
import { ALObjectWizardPage } from "./alObjectWizardPage";
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { ALPermissionSetSyntaxBuilder } from '../syntax_builders/alPermissionSetSyntaxBuilder';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSPIObjectListItemHelper } from '../../../langserver/project_information/symbols/lspiObjectListItemHelper';

export class ALPermissionSetWizardPage extends ALObjectWizardPage {
    private _permissionSetWizardData : ALPermissionSetWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, title: string | undefined, settings: ALObjectWizardSettings, data : ALPermissionSetWizardData) {
        if (!title) {
            title = "AL Permission Set Wizard";
        }
        super(toolsExtensionContext, title, "azALDevTools.ALPermissionSetWizard", settings, data);
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
    
        await this.finishObjectIdReservation(this._permissionSetWizardData);

        //get all extension objects
        if (this._permissionSetWizardData.inclAllObjects) {
            let allObjects = await this.context.projectInformationService.getFilteredObjectList(this._settings.destDirectoryUri, {
                kind: LSObjectKind.Unknown,
                excludeFullInherentPermissions: true,
                skipDependencies: true
            });
            this._permissionSetWizardData.selectedObjectsList = LSPIObjectListItemHelper.toObjectIdentifierList(allObjects);
        }

        //get namespaces information
        let referencedObjects = this.collectReferencedObjects();
        let fileNamespaces = await this.getNamespaceAndUsings(LSObjectKind.PermissionSet, this._permissionSetWizardData.objectName, this._permissionSetWizardData.objectNamespace, referencedObjects);
        if (fileNamespaces) {            
            this._permissionSetWizardData.objectNamespace = fileNamespaces.namespace;
            this._permissionSetWizardData.objectUsings = fileNamespaces.usings;
            this.addReferencedObjectsNamespaces();
        }
    }

    protected collectReferencedObjects(): LSPIObjectIdentifier[] {
        let referencedObjects: LSPIObjectIdentifier[] = [];
        
        if (this._permissionSetWizardData.selectedObjectsList) {
            for (let i=0; i<this._permissionSetWizardData.selectedObjectsList.length; i++) {
                referencedObjects.push(this._permissionSetWizardData.selectedObjectsList[i]);
            }
        }

        if (this._permissionSetWizardData.selectedPermissionSetList) {
            for (let i=0; i<this._permissionSetWizardData.selectedPermissionSetList.length; i++) {
                referencedObjects.push(this._permissionSetWizardData.selectedPermissionSetList[i]);
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

    protected runBuilder() {
        //build new object
        var builder : ALPermissionSetSyntaxBuilder = new ALPermissionSetSyntaxBuilder();
        var source = builder.buildFromPermissionSetWizardData(this._settings.destDirectoryUri, this._permissionSetWizardData, this._settings.projectProfile);
        this.createObjectFile(LSObjectKind.PermissionSet, 0, this._permissionSetWizardData.objectName, source);
    }

    protected async loadPermissionSets() {      
        this._permissionSetWizardData.permissionSetList = await this.context.projectInformationService.getObjectList(this._settings.destDirectoryUri, LSObjectKind.PermissionSet);

        //let resourceUri = this._settings.getDestDirectoryUri();
        //this._wizardData.codeunitList = await this._toolsExtensionContext.alLangProxy.getCodeunitList(resourceUri);
        if ((this._permissionSetWizardData.permissionSetList) && (this._permissionSetWizardData.permissionSetList.length > 0)) {
            this.sendMessage({
                command : "setPermissionSets",
                data : this._permissionSetWizardData.permissionSetList
            });
        }
    }

}