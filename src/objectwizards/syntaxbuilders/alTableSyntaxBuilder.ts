import * as vscode from 'vscode';
import { ALTableWizardData } from "../wizards/alTableWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALSyntaxHelper } from '../../allanguage/alSyntaxHelper';

export class ALTableSyntaxBuilder {
    constructor() {
    }

    buildFromTableWizardData(destUri: vscode.Uri | undefined, data: ALTableWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartObject("table", data.objectId, data.objectName);
        writer.addProperty("Caption", writer.encodeString(ALSyntaxHelper.removePrefixSuffix(data.objectName, data.projectSettings)));
        writer.addProperty("DataClassification", "ToBeClassified");
        if (!data.dataPerCompany)
            writer.addProperty("DataPerCompany", "false");
        writer.writeProperties();
        writer.writeLine("");

        //write fields here
        writer.writeStartFields();

        for (let i=0; i<data.fields.length; i++) {
            writer.writeTableField(data.fields[i].id, data.fields[i].name, data.fields[i].type, data.fields[i].length,
                data.fields[i].dataClassification);
        }

        writer.writeEndFields();

        //suggest keys
        writer.writeLine("keys");
        writer.writeStartBlock();

        if (data.fields.length > 0) {
            //collect primary keys
            let pkFields: string = "";
            let hasPKFields: boolean = false;
            data.fields.forEach((item, index) => {
                if (item.pk) {
                    if (hasPKFields)
                        pkFields = pkFields + ",";
                    pkFields = pkFields + writer.encodeName(item.name);
                    hasPKFields = true;
                }
            });
            if (!hasPKFields)
                pkFields = writer.encodeName(data.fields[0].name);

            writer.writeStartNameSourceBlock("key", "PK", pkFields);
            writer.writeProperty("Clustered", "true");
            writer.writeEndBlock();
        }

        writer.writeEndBlock();

        writer.writeLine("");
        writer.writeEndObject();

        return writer.toString();
    }

}