import * as vscode from 'vscode';
import { ALReportWizardData } from "../wizards/alReportWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALSyntaxHelper } from '../../allanguage/alSyntaxHelper';

export class ALReportSyntaxBuilder {
    
    constructor() {
    }

    buildFromReportWizardData(destUri: vscode.Uri | undefined, data : ALReportWizardData) : string {
        let settings =  vscode.workspace.getConfiguration('alOutline', destUri);
        let addDataItemName = settings.get<boolean>('addDataItemToReportColumnName');

        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartObject("report", data.objectId, data.objectName);

        writer.addProperty("Caption", writer.encodeString(ALSyntaxHelper.removePrefixSuffix(data.objectName, data.projectSettings)));

        //write layout path
        if (data.rdlcLayout != "")
            writer.addProperty("RDLCLayout", writer.encodeString(data.rdlcLayout));
        if (data.wordLayout != "")
            writer.addProperty("WordLayout", writer.encodeString(data.wordLayout));

        if ((data.usageCategory) && (data.usageCategory !== "") && (data.usageCategory !== "None")) {
            //application area requires useage category to be set
            if ((data.applicationArea) && (data.applicationArea !== ""))
                writer.addProperty("ApplicationArea", data.applicationArea);
            writer.addProperty("UsageCategory", data.usageCategory);
        }

        writer.writeProperties();

        //write dataset
        this.writeDataSet(writer, data, addDataItemName);

        //write report request page suggetsion
        if (data.createRequestPage)
            this.writeRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private writeDataSet(writer : ALSyntaxWriter, data : ALReportWizardData, addDataItemName: boolean | undefined) {
        let dataSetName = writer.createName(data.selectedTable);
        writer.writeStartNamedBlock("dataset");

        writer.writeStartNameSourceBlock("dataitem", dataSetName, writer.encodeName(data.selectedTable));

        if (data.selectedFieldList) {
            for (let i=0; i<data.selectedFieldList.length; i++) {
                let columnName = writer.createName(data.selectedFieldList[i].name!);
                if (addDataItemName)
                    columnName = columnName + "_" + dataSetName;
                
                writer.writeNameSourceBlock("column", columnName, 
                    writer.encodeName(data.selectedFieldList[i].name!));
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