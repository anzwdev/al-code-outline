import * as vscode from 'vscode';
import { ObjectBuilder } from "./objectBuilder";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALObjectWriter } from './alObjectWriter';
import { ALSymbolKind } from '../alSymbolKind';

export class XmlPortBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showXmlPortWizard(tableSymbol : ALSymbolInfo) {
        let objectName : string = await this.getXmlPortName(
            tableSymbol.symbolName.trim() + " XmlPort");
        
        if (!objectName)
            return;

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
        
        this.showNewDocument(this.buildXmlPortForTable(tableSymbol, 0, objectName, (selectedNodeType.label == fieldAsElementText)));
    }

    //#endregion

    //#region UI functions

    private getXmlPortName(defaultName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultName,
            prompt : "Please enter new xmlport name"});
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

        tableSymbol.childItems.forEach(
            item => {
                if (item.alKind == ALSymbolKind.Field) {
                    writer.writeNameSourceBlock(fieldNodeName, writer.createName(item.symbolName), tableElementName + "." + writer.encodeName(item.symbolName));
                }
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