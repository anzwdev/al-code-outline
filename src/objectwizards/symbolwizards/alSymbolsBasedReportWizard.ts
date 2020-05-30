import { FileBuilder } from '../fileBuilder';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSymbolsBasedWizard } from './alSymbolsBasedWizard';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';

export class ALSymbolsBasedReportWizard extends ALSymbolsBasedWizard {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showWizard(tableSymbols: AZSymbolInformation[]) {
        if (tableSymbols.length == 1)
            await this.showReportWizard(tableSymbols[0]);
        else
            await this.showMultiReportWizard(tableSymbols);
    }


    async showMultiReportWizard(tableSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const objType : AZSymbolKind = AZSymbolKind.ReportObject;

        let startObjectId: number = await this.getObjectId(`Please enter a starting ID for the report objects.`, 0);
        if (startObjectId < 0) {
            return;
        }

        let relativeFileDir = await this.getRelativeFileDir(objType);

        for (let i = 0; i < tableSymbols.length; i++) {
            let tableSymbol = tableSymbols[i];
            let objectId: number = startObjectId + i;
            let objectName : string = this.getDefaultReportName(tableSymbol);

            await this.createAndShowNewReport(tableSymbol, objType, objectId, objectName, relativeFileDir);
        }
    }

    async showReportWizard(tableSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsFileNamePatternRequired())
            return;
            
        const objType : AZSymbolKind = AZSymbolKind.ReportObject;

        let objectId : number = await this.getObjectId("Please enter an ID for the report object.", 0);
        if (objectId < 0) {
            return;
        }

        let objectName = await this.getObjectName("Please enter a name for the report object.", this.getDefaultReportName(tableSymbol));
        if (!objectName)
            return;

        let relativeFileDir = await this.getRelativeFileDir(objType);
        await this.createAndShowNewReport(tableSymbol, objType, objectId, objectName, relativeFileDir);
    }

    private async createAndShowNewReport(tableSymbol: AZSymbolInformation, objType: AZSymbolKind, objectId: number, objectName: string, relativeFileDir: string | undefined) {
        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildReportForTable(tableSymbol, objectId, objectName), fileName, relativeFileDir);
    }

    //#endregion
  
    //#region Report builders

    buildReportForTable(tableSymbol : AZSymbolInformation, objectId : number, objectName : string) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(undefined);

        writer.writeStartObject("report", objectId.toString(), objectName);

        //write dataset
        this.appendDataSet(writer, tableSymbol);

        //write report request page suggetsion
        this.appendReportRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private appendDataSet(writer : ALSyntaxWriter, tableSymbol : AZSymbolInformation) {
        var dataSetName = writer.createName(tableSymbol.name);
        writer.writeStartNamedBlock("dataset");

        writer.writeStartNameSourceBlock("dataitem", dataSetName, writer.encodeName(tableSymbol.name));

        let fieldList : AZSymbolInformation[] = [];
        tableSymbol.collectChildSymbols(AZSymbolKind.Field, true, fieldList);
        fieldList.forEach(
            item => {
                writer.writeNameSourceBlock("column", writer.createName(item.name), writer.encodeName(item.name));
            }
        );

        writer.writeEndBlock();

        writer.writeEndBlock();
    }

    private appendReportRequestPage(writer : ALSyntaxWriter) {
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

    private getDefaultReportName(tableSymbol: AZSymbolInformation) : string {
        return `${tableSymbol.name.trim()} Report`;
    }
    
    //#endregion
}