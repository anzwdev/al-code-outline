import { FileBuilder } from '../fileBuilder';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSymbolsBasedWizard } from './alSymbolsBasedWizard';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';

export class ALSymbolsBasedQueryWizard extends ALSymbolsBasedWizard {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showWizard(tableSymbols: AZSymbolInformation[]) {
        if (tableSymbols.length == 1)
            await this.showQueryWizard(tableSymbols[0]);
        else
            await this.showMultiQueryWizard(tableSymbols);
    }

    async showMultiQueryWizard(tableSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const objType : AZSymbolKind = AZSymbolKind.QueryObject;

        let startObjectId: number = await this.getObjectId(`Please enter a starting ID for the query objects.`, 0);
        if (startObjectId < 0) {
            return;
        }

        let relativeFileDir = await this.getRelativeFileDir(objType);

        for (let i = 0; i < tableSymbols.length; i++) {
            let tableSymbol = tableSymbols[i];
            let objectId: number = startObjectId + i;
            let objectName : string = this.getDefaultQueryName(tableSymbol);

            await this.createAndShowNewQuery(tableSymbol, objType, objectId, objectName, relativeFileDir);
        }
    }

    async showQueryWizard(tableSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsFileNamePatternRequired())
            return;
            
        const objType : AZSymbolKind = AZSymbolKind.QueryObject;

        let objectId : number = await this.getObjectId("Please enter an ID for the query object.", 0);
        if (objectId < 0) {
            return;
        }

        let objectName : string | undefined = this.getDefaultQueryName(tableSymbol);
        objectName = await this.getObjectName("Please enter a name for the query object.", objectName);
        
        if (!objectName) {
            return;
        }

        let relativeFileDir = await this.getRelativeFileDir(objType);
        await this.createAndShowNewQuery(tableSymbol, objType, objectId, objectName, relativeFileDir);
    }

    private async createAndShowNewQuery(tableSymbol: AZSymbolInformation, objType: AZSymbolKind, objectId: number, objectName: string, relativeFileDir: string | undefined) {
        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildQueryForTable(tableSymbol, objectId, objectName), fileName, relativeFileDir);
    }

    //#endregion
      
    //#region Query builders

    buildQueryForTable(tableSymbol : AZSymbolInformation, objectId : number, objectName : string) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(undefined);

        writer.writeStartObject("query", objectId.toString(), objectName);
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

    private appendElements(writer : ALSyntaxWriter, tableSymbol : AZSymbolInformation) {
        var dataItemName = writer.createName(tableSymbol.name);
        writer.writeStartNamedBlock("elements");

        writer.writeStartNameSourceBlock("dataitem", dataItemName, writer.encodeName(tableSymbol.name));

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
        
    //#endregion

    //#region Helper Methods

    private getDefaultQueryName(tableSymbol: AZSymbolInformation) : string {
        return `${tableSymbol.name.trim()} Query`;
    }
    
    //#endregion
}