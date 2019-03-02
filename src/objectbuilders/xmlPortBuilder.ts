import * as vscode from 'vscode';
import { ALObjectWriter } from './alObjectWriter';
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from "./objectBuilder";

export class XmlPortBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showXmlPortWizard(tableSymbol : ALSymbolInfo) {
        const objType : ALSymbolKind = ALSymbolKind.XmlPort;

        let objectId : number = await this.getObjectId("Please enter an ID for the xmlport object.", 0);

        let objectName : string = tableSymbol.symbolName.trim() + " XmlPort";
        objectName = await this.getObjectName("Please enter a name for the xmlport object.", objectName);
        
        if (!objectName) {
            return;
        }

        //ask user to select table field node type
        let fieldAsAttributeText = "Table fields as xml attributes";
        let fieldAsElementText = "Table fields as xml elements";

        let fieldNodeTypes = [
            { 
                label: fieldAsAttributeText, 
                description: ""
            },
            {
                label : fieldAsElementText,
                description : ""
            }];

        let selectedNodeType = await vscode.window.showQuickPick(fieldNodeTypes);
        if ((!selectedNodeType) || (!selectedNodeType.label))
            return;
        
        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildXmlPortForTable(tableSymbol, objectId, objectName, (selectedNodeType.label == fieldAsElementText)), fileName, objType);
    }

    //#endregion
  
    //#region Report builders

    buildXmlPortForTable(tableSymbol : ALSymbolInfo, objectId : number, objectName : string, fieldsAsElements : boolean) : string {
        //generate file content
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartObject("xmlport", objectId, objectName);

        //write dataset
        this.appendSchema(writer, tableSymbol, fieldsAsElements);

        //write report request page suggetsion
        this.appendReportRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private appendSchema(writer : ALObjectWriter, tableSymbol : ALSymbolInfo, fieldsAsElements : boolean) {       
        var tableElementName = writer.createName(tableSymbol.symbolName);
        var fieldNodeName : string;
        if (fieldsAsElements)
            fieldNodeName = "fieldelement";
        else
            fieldNodeName = "fieldattribute";

        writer.writeStartNamedBlock("schema");
        writer.writeStartGroup("textelement", "RootNodeName");

        writer.writeStartNameSourceBlock("tableelement", tableElementName, writer.encodeName(tableSymbol.symbolName));

        let fieldList : ALSymbolInfo[] = [];
        tableSymbol.getAllSymbolsByKind(ALSymbolKind.Field, fieldList);
        fieldList.forEach(
            item => {
                writer.writeNameSourceBlock(fieldNodeName, writer.createName(item.symbolName), tableElementName + "." + writer.encodeName(item.symbolName));
            }
        );

        writer.writeEndBlock();

        writer.writeEndBlock();
        writer.writeEndBlock();
    }

    private appendReportRequestPage(writer : ALObjectWriter) {
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

    //#endregion

}