import * as vscode from 'vscode';
import { ALObjectWriter } from './alObjectWriter';
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from "./objectBuilder";

export class QueryBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showQueryWizard(tableSymbol : ALSymbolInfo) {
        const objType : ALSymbolKind = ALSymbolKind.Query;

        let objectId : number = await this.getObjectId("Please enter an ID for the query object.", 0);
        if (objectId < 0) {
            return;
        }

        let objectName : string = tableSymbol.symbolName.trim() + " Query";
        objectName = await this.getObjectName("Please enter a name for the query object.", objectName);
        
        if (!objectName) {
            return;
        }

        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildQueryForTable(tableSymbol, objectId, objectName), fileName, objType);
    }

    //#endregion
      
    //#region Query builders

    buildQueryForTable(tableSymbol : ALSymbolInfo, objectId : number, objectName : string) : string {
        //generate file content
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartObject("query", objectId, objectName);
        writer.writeProperty("QueryType", "Normal");
        writer.writeLine("");

        //write dataset
        this.appendElements(writer, tableSymbol);

        //write triggers
        writer.writeLine("");
        writer.writeLine("trigger OnBeforeOpen()");
        writer.writeLine("begin");
        writer.writeLine("");                
        writer.writeLine("end;");
        
        writer.writeEndObject();
        
        return writer.toString();
    }

    private appendElements(writer : ALObjectWriter, tableSymbol : ALSymbolInfo) {
        var dataItemName = writer.createName(tableSymbol.symbolName);
        writer.writeStartNamedBlock("elements");

        writer.writeStartNameSourceBlock("dataitem", dataItemName, writer.encodeName(tableSymbol.symbolName));

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
        
    //#endregion

}