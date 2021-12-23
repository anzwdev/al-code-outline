import * as vscode from 'vscode';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { ALPermissionSetExtensionWizardData } from "../wizards/alPermissionSetExtensionWizardData";
import { ALPermissionSetSyntaxBuilder } from "./alPermissionSetSyntaxBuilder";

export class ALPermissionSetExtensionSyntaxBuilder extends ALPermissionSetSyntaxBuilder {

    buildFromPermissionSetExtWizardData(destUri: vscode.Uri | undefined, data : ALPermissionSetExtensionWizardData) : string {
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);
       
        writer.writeStartExtensionObject("permissionsetextension", data.objectId, data.objectName, data.basePermissionSet);

        this.writeIncludePermissionSetList(writer, data.selectedPermissionSetList);
        this.writePermissions(writer, data.selectedObjectsList);

        //finish object
        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }

}