import * as vscode from 'vscode';
import { ObjectBuilder } from "./objectBuilder";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALObjectWriter } from './alObjectWriter';
import { ALSymbolKind } from '../alSymbolKind';

export class QueryBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showQueryWizard(tableSymbol : ALSymbolInfo) {
        let objectName : string = await this.getQueryName(
            tableSymbol.symbolName.trim() + " Query");
        
        if (!objectName)
            return;

        this.showNewDocument(this.buildQueryForTable(tableSymbol, 0, objectName));
    }

    //#endregion
    
    //#region UI functions

    private getQueryName(defaultName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultName,
            prompt : "Please enter new query name"});
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

        tableSymbol.childItems.forEach(
            item => {
                if (item.alKind == ALSymbolKind.Field) {
                    writer.writeNameSourceBlock("column", writer.createName(item.symbolName), writer.encodeName(item.symbolName));
                }
            }
        );

        writer.writeEndBlock();

        writer.writeEndBlock();
    }
        
    //#endregion

}