import * as vscode from 'vscode';
import { ALSyntaxHelper } from '../../allanguage/alSyntaxHelper';
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ObjectInformation } from '../../symbolsinformation/objectInformation';
import { ALPermissionSetWizardData } from "../wizards/alPermissionSetWizardData";

export class ALPermissionSetSyntaxBuilder {

    buildFromPermissionSetWizardData(destUri: vscode.Uri | undefined, data : ALPermissionSetWizardData) : string {
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartObject("permissionset", data.objectId, data.objectName);
        writer.addProperty("Assignable", "true");

        if ((data.objectCaption) && (data.objectCaption.length > 0))
            writer.addProperty("Caption", writer.encodeString(data.objectCaption));
        else
            writer.addProperty("Caption", writer.encodeString(ALSyntaxHelper.removePrefixSuffix(data.objectName, data.projectSettings)));
        writer.writeProperties();
        
        this.writeIncludePermissionSetList(writer, data.selectedPermissionSetList);

        this.writePermissions(writer, data.selectedObjectsList);

        writer.writeLine("");

        //finish object
        writer.writeEndObject();
        
        return writer.toString();
    }

    protected writeIncludePermissionSetList(writer: ALSyntaxWriter, list: string[] | undefined) {
        if ((list) && (list.length > 0)) {
            writer.writeStartProperty("IncludedPermissionSets");
            for (let i=0; i<list.length; i++) {
                writer.writePropertyValue(ALSyntaxHelper.toNameText(list[i]), (i == (list.length - 1)));
            }
        }
    }

    protected writePermissions(writer: ALSyntaxWriter, list: ObjectInformation[] | undefined) {
        if ((list) && (list.length > 0)) {
            writer.writeStartProperty("Permissions");
            for (let i=0; i<list.length; i++) {
                this.writeObjectPermission(writer, list[i], (i == (list.length - 1)));
            }
        }
    }

    protected writeObjectPermission(writer: ALSyntaxWriter, objectInformation: ObjectInformation, lastValue: boolean) {
        if ((objectInformation.name) && (objectInformation.type)) {
            let name = ALSyntaxHelper.toNameText(objectInformation.name);
            if (objectInformation.type == "Table") {
                this.writeObjectPermissionLine(writer, "table", name, "X", false);
                this.writeObjectPermissionLine(writer, "tabledata", name, "RMID", lastValue);
            } else {
                this.writeObjectPermissionLine(writer, objectInformation.type.toLowerCase(), name, "X", lastValue);
            }
        }
        
    }

    protected writeObjectPermissionLine(writer: ALSyntaxWriter, type: string, name: string, permissions: string, lastValue: boolean) {
        writer.writePropertyValue(type + " " + name + " = " + permissions, lastValue);
    }

}