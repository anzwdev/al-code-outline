import * as vscode from 'vscode';
import { ALXmlPortWizardData } from "../wizards/alXmlPortWizardData";
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";

export class ALXmlPortSyntaxBuilder {

    constructor() {
    }

    buildFromXmlPortWizardData(destUri: vscode.Uri | undefined, data : ALXmlPortWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);

        writer.writeStartObject("xmlport", data.objectId, data.objectName);

        //write dataset
        this.writeSchema(writer, data);

        //write report request page suggestion
        if (data.createRequestPage)
            this.writeRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private writeSchema(writer : ALSyntaxWriter, data : ALXmlPortWizardData) {       
        let tableElementName = writer.createName(data.selectedTable);
        let fieldNodeName : string;
        if (data.fieldNodeType == "element")
            fieldNodeName = "fieldelement";
        else
            fieldNodeName = "fieldattribute";

        writer.writeStartNamedBlock("schema");
        writer.writeStartGroup("textelement", "RootNodeName");

        writer.writeStartNameSourceBlock("tableelement", tableElementName, writer.encodeName(data.selectedTable));

        if (data.selectedFieldList)
        {
            for (let i=0; i<data.selectedFieldList.length;i++) {
                writer.writeNameSourceBlock(fieldNodeName, 
                    writer.createName(data.selectedFieldList[i]), 
                    tableElementName + "." + writer.encodeName(data.selectedFieldList[i]));
            }
        }

        writer.writeEndBlock();

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
