import * as path from 'path';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALPermissionSetExtensionSyntaxBuilder } from '../syntaxbuilders/alPermissionSetExtensionSyntaxBuilder';
import { ALObjectWizardSettings } from "./alObjectWizardSettings";
import { ALPermissionSetExtensionWizardData } from "./alPermissionSetExtensionWizardData";
import { ALPermissionSetWizardPage } from "./alPermissionSetWizardPage";
import { ToolsSymbolReference } from '../../langserver/symbolsinformation/toolsSymbolReference';

export class ALPermissionSetExtensionWizardPage extends ALPermissionSetWizardPage {
    private _permissionSetExtensionWizardData : ALPermissionSetExtensionWizardData;

    constructor(toolsExtensionContext : DevToolsExtensionContext, settings: ALObjectWizardSettings, data : ALPermissionSetExtensionWizardData) {
        super(toolsExtensionContext, "AL Perm. Set Ext. Wizard", settings, data);
        this._permissionSetExtensionWizardData = data;
    }

    protected getHtmlContentPath() : string {
        return path.join('htmlresources', 'alpermissionsetextwizard', 'alpermissionsetextwizard.html');
    }

    protected getViewType() : string {
        return "azALDevTools.ALPermissionSetExtensionWizard";
    }
    
    protected async setBuilderData(data: any) {
        this._permissionSetExtensionWizardData.basePermissionSet = data.basePermissionSet;
        await super.setBuilderData(data);
    }

    protected collectReferencedObjects(): ToolsSymbolReference[] {
        let referencedObjects = super.collectReferencedObjects();
        if (this._permissionSetExtensionWizardData.basePermissionSet) {
            referencedObjects.push({
                nameWithNamespaceOrId: this._permissionSetExtensionWizardData.basePermissionSet,
                typeName: 'PermissionSet'
            });
        }
        return referencedObjects;
    }

    protected getWizardObjectType(): string {
        return 'PermissionSetExtension';
    }

    protected runBuilder() {
        //build new object
        var builder : ALPermissionSetExtensionSyntaxBuilder = new ALPermissionSetExtensionSyntaxBuilder();
        var source = builder.buildFromPermissionSetExtWizardData(this._settings.getDestDirectoryUri(), this._permissionSetExtensionWizardData);
        this.createObjectExtensionFile('PermissionSetExtension', this._permissionSetExtensionWizardData.objectId, 
            this._permissionSetExtensionWizardData.objectName, this._permissionSetExtensionWizardData.basePermissionSet, source);
    }

}