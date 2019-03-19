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

    async showMultiReportWizard(tableSymbols: ALSymbolInfo[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const objType : ALSymbolKind = ALSymbolKind.Report;

        let startObjectId: number = await this.getObjectId(`Please enter a starting ID for the report objects.`, 0);
        if (startObjectId < 0) {
            return;
        }

        let relativeFileDir: string = await this.getRelativeFileDir(objType);

        for (let i = 0; i < tableSymbols.length; i++) {
            let tableSymbol = tableSymbols[i];
            let objectId: number = startObjectId + i;
            let objectName : string = this.getDefaultReportName(tableSymbol);

            await this.createAndShowNewReport(tableSymbol, objType, objectId, objectName, relativeFileDir);
        }
    }

    async showReportWizard(tableSymbol : ALSymbolInfo) {
        if (!FileBuilder.checkCrsFileNamePatternRequired())
            return;
            
        const objType : ALSymbolKind = ALSymbolKind.Report;

        let objectId : number = await this.getObjectId("Please enter an ID for the report object.", 0);
        if (objectId < 0) {
            return;
        }

        let objectName : string = this.getDefaultReportName(tableSymbol);
        objectName = await this.getObjectName("Please enter a name for the report object.", objectName);
        
        if (!objectName) {
            return;
        }

        let relativeFileDir: string = await this.getRelativeFileDir(objType);
        await this.createAndShowNewReport(tableSymbol, objType, objectId, objectName, relativeFileDir);
    }

    private async createAndShowNewReport(tableSymbol: ALSymbolInfo, objType: ALSymbolKind, objectId: number, objectName: string, relativeFileDir: string) {
        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildReportForTable(tableSymbol, objectId, objectName), fileName, relativeFileDir);
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

    //#region Helper Methods

    private getDefaultReportName(tableSymbol: ALSymbolInfo) : string {
        return `${tableSymbol.symbolName.trim()} Report`;
    }
    
    //#endregion
}