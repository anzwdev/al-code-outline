import * as vscode from 'vscode';
import { ALReportWizardData } from "../wizards/alReportWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";

export class ALReportSyntaxBuilder {
    
    constructor() {
    }

    buildFromReportWizardData(destUri: vscode.Uri | undefined, data : ALReportWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartObject("report", data.objectId, data.objectName);

        //write layout path
        if (data.rdlcLayout != "")
            writer.writeProperty("RDLCLayout", writer.encodeString(data.rdlcLayout));
        if (data.wordLayout != "")
            writer.writeProperty("WordLayout", writer.encodeString(data.wordLayout));

        //write dataset
        this.writeDataSet(writer, data);

        //write report request page suggetsion
        if (data.createRequestPage)
            this.writeRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private writeDataSet(writer : ALSyntaxWriter, data : ALReportWizardData) {
        let dataSetName = writer.createName(data.selectedTable);
        writer.writeStartNamedBlock("dataset");

        writer.writeStartNameSourceBlock("dataitem", dataSetName, writer.encodeName(data.selectedTable));

        if (data.selectedFieldList) {
            for (let i=0; i<data.selectedFieldList.length; i++) {
                writer.writeNameSourceBlock("column", writer.createName(data.selectedFieldList[i]), 
                    writer.encodeName(data.selectedFieldList[i]));
            }
        }

        writer.writeEndBlock();

        writer.writeEndBlock();
    }

    private writeRequestPage(writer : ALSyntaxWriter) {
        writer.writeStartNamedBlock("requestpage");

        //layout
        writer.writeStartLayout();
        writer.writeStartGroup("area", "content");
        writer.writeStartGroup("group", "GroupName");
        writer.writeEndBlock();
        writer.writeEndBlock();
        writer.writeEndLayout();

        //actions
        writer.writeStartNamedBlock("actions")
        writer.writeStartGroup("area", "processing");
        writer.writeEndBlock();
        writer.writeEndBlock();

        writer.writeEndBlock();
    }

} 