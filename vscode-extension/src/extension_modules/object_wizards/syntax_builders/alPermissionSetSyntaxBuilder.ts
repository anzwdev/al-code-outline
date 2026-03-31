import * as vscode from 'vscode';
import { ALPermissionSetWizardData } from "../wizards/alPermissionSetWizardData";
import { ALSyntaxWriter } from '../../../al_syntax/alSyntaxWriter';
import { ALSyntaxHelper } from '../../../al_syntax/alSyntaxHelper';
import { LSPIProjectProfile } from '../../../langserver/project_information/profile/lspiProjectProfile';
import { LSPIObjectIdentifier } from '../../../langserver/project_information/symbols/lspiObjectIdentifier';
import { LSObjectKind } from '../../../langserver/common_types/lsObjectKind';
import { LSObjectKindHelper } from '../../../langserver/common_types/lsObjectKindHelper';

export class ALPermissionSetSyntaxBuilder {

    buildFromPermissionSetWizardData(destUri: vscode.Uri | undefined, data : ALPermissionSetWizardData, projectProfile: LSPIProjectProfile | undefined) : string {
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeNamespace(data.objectNamespace);
        writer.writeUsings(data.objectUsings);

        writer.writeStartObject("permissionset", data.objectId, data.objectName);
        writer.addProperty("Assignable", "true");

        if ((data.objectCaption) && (data.objectCaption.length > 0)) {
            writer.addProperty("Caption", writer.encodeString(data.objectCaption) + ', MaxLength = 30');
        } else {
            writer.addProperty("Caption", writer.encodeString(ALSyntaxHelper.removePrefixSuffix(data.objectName, projectProfile?.affixes)) + ', MaxLength = 30');
        }
        writer.writeProperties();
        
        this.writeIncludePermissionSetList(writer, data.selectedPermissionSetList);

        this.writePermissions(writer, data.selectedObjectsList);

        //finish object
        writer.writeEndObject();
        
        return writer.toWizardGeneratedString();
    }

    protected writeIncludePermissionSetList(writer: ALSyntaxWriter, list: LSPIObjectIdentifier[] | undefined) {
        if ((list) && (list.length > 0)) {
            writer.writeStartProperty("IncludedPermissionSets");
            for (let i=0; i<list.length; i++) {
                writer.writePropertyValue(ALSyntaxHelper.toNameText(list[i].name), (i === (list.length - 1)));
            }
        }
    }

    protected writePermissions(writer: ALSyntaxWriter, list: LSPIObjectIdentifier[] | undefined) {
        if ((list) && (list.length > 0)) {
            writer.writeStartProperty("Permissions");
            for (let i=0; i<list.length; i++) {
                this.writeObjectPermission(writer, list[i], (i === (list.length - 1)));
            }
        }
    }

    protected writeObjectPermission(writer: ALSyntaxWriter, objectInformation: LSPIObjectIdentifier, lastValue: boolean) {
        if (objectInformation.name) {
            let name = ALSyntaxHelper.toNameText(objectInformation.name);
            if (objectInformation.kind === LSObjectKind.Table) {
                this.writeObjectPermissionLine(writer, "table", name, "X", false);
                this.writeObjectPermissionLine(writer, "tabledata", name, "RMID", lastValue);
            } else {
                let typeName = LSObjectKindHelper.toObjectTypeName(objectInformation.kind) ?? "";
                this.writeObjectPermissionLine(writer, typeName.toLowerCase(), name, "X", lastValue);
            }
        }
        
    }

    protected writeObjectPermissionLine(writer: ALSyntaxWriter, type: string, name: string, permissions: string, lastValue: boolean) {
        writer.writePropertyValue(type + " " + name + " = " + permissions, lastValue);
    }

}