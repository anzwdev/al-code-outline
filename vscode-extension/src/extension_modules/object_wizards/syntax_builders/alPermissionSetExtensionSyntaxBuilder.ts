import * as vscode from 'vscode';
import { ALPermissionSetExtensionWizardData } from "../wizards/alPermissionSetExtensionWizardData";
import { ALPermissionSetSyntaxBuilder } from "./alPermissionSetSyntaxBuilder";
import { ALSyntaxWriter } from '../../../al_syntax/alSyntaxWriter';

export class ALPermissionSetExtensionSyntaxBuilder extends ALPermissionSetSyntaxBuilder {

    buildFromPermissionSetExtWizardData(destUri: vscode.Uri | undefined, data : ALPermissionSetExtensionWizardData) : string {
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);
        
        writer.writeStartExtensionObject("permissionsetextension", data.objectId, data.objectName, data.basePermissionSet?.name ?? "");

        this.writeIncludePermissionSetList(writer, data.selectedPermissionSetList);
        this.writePermissions(writer, data.selectedObjectsList);

        //finish object
        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }

}