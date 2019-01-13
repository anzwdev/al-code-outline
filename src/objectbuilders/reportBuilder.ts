import * as vscode from 'vscode';
import { ALObjectWriter } from './alObjectWriter';
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from "./objectBuilder";

export class ReportBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showReportWizard(tableSymbol : ALSymbolInfo) {
        const objType : ALSymbolKind = ALSymbolKind.Report;

        let objectId : number = await this.getObjectId("Please enter an ID for the report object.", 0);

        let objectName : string = tableSymbol.symbolName.trim() + " Report";
        objectName = await this.getObjectName("Please enter a name for the report object.", objectName);
        
        if (!objectName) {
            return;
        }

        let fileName : string = FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildReportForTable(tableSymbol, objectId, objectName), fileName, objType);
    }

    //#endregion
  
    //#region Report builders

    buildReportForTable(tableSymbol : ALSymbolInfo, objectId : number, objectName : string) : string {
        //generate file content
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartObject("report", objectId, objectName);

        //write dataset
        this.appendDataSet(writer, tableSymbol);

        //write report request page suggetsion
        this.appendReportRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private appendDataSet(writer : ALObjectWriter, tableSymbol : ALSymbolInfo) {
        var dataSetName = writer.createName(tableSymbol.symbolName);
        writer.writeStartNamedBlock("dataset");

        writer.writeStartNameSourceBlock("dataitem", dataSetName, writer.encodeName(tableSymbol.symbolName));

        let fieldList : ALSymbolInfo[] = [];
        tableSymbol.getAllSymbolsByKind(ALSymbolKind.Field, fieldList);
        fieldList.forEach(
            item => {
                writer.writeNameSourceBlock("column", writer.createName(item.symbolName), writer.encodeName(item.symbolName));
            }
        );

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