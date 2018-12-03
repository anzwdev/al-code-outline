import * as vscode from 'vscode';
import { ObjectBuilder } from "./objectBuilder";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALObjectWriter } from './alObjectWriter';
import { ALSymbolKind } from '../alSymbolKind';

export class ReportBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showReportWizard(tableSymbol : ALSymbolInfo) {
        let objectName : string = await this.getReportName(
            tableSymbol.symbolName.trim() + " Report");
        
        if (!objectName)
            return;

        this.showNewDocument(this.buildReportForTable(tableSymbol, 0, objectName));
    }

    //#endregion

    //#region UI functions

    private getReportName(defaultName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultName,
            prompt : "Please enter new report name"});
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