import * as vscode from 'vscode';
import { ALTableExtWizardData } from "../wizards/alTableExtWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";

export class ALTableExtSyntaxBuilder {
    constructor() {
    }

    buildFromTableExtWizardData(destUri: vscode.Uri | undefined, data: ALTableExtWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartExtensionObject("tableextension", data.objectId, data.objectName, data.selectedTable);

        //write fields here
        writer.writeStartFields();

        for (let i = 0; i < data.fields.length; i++) {
            writer.writeTableField(data.fields[i].id, data.fields[i].name, data.fields[i].type, data.fields[i].length,
                data.fields[i].dataClassification);
        }

        writer.writeEndFields();

        writer.writeEndObject();

        return writer.toWizardGeneratedString();
    }
}