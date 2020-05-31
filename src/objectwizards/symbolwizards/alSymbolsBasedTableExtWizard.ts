import * as vscode from 'vscode';
import { FileBuilder } from '../fileBuilder';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSymbolsBasedWizard } from './alSymbolsBasedWizard';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';

export class ALSymbolsBasedTableExtWizard extends ALSymbolsBasedWizard {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showWizard(tableSymbols: AZSymbolInformation[]) {
        if (tableSymbols.length == 1)
            await this.showTableExtWizard(tableSymbols[0]);
        else
            await this.showMultiTableExtWizard(tableSymbols);
    }

    async showMultiTableExtWizard(tableSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(true))
            return;

        const extObjType : AZSymbolKind = AZSymbolKind.TableExtensionObject;

        let startObjectId: number = await this.getObjectId("Please enter a starting ID for the table extensions.", 0);
        if (startObjectId < 0) {
            return;
        }

        let relativeFileDir = await this.getRelativeFileDir(extObjType);

        for (let i = 0; i < tableSymbols.length; i++) {
            let tableSymbol = tableSymbols[i];
            let extObjectId: number = startObjectId + i;
            let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, tableSymbol);

            await this.createAndShowNewTableExtension(tableSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
        }
    }

    async showTableExtWizard(tableSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(false))
            return;
            
        const extObjType : AZSymbolKind = AZSymbolKind.TableExtensionObject;

        let extObjectId : number = await this.getObjectId("Please enter an ID for the table extension.", 0);
        if (extObjectId < 0) {
            return;
        }

        let extObjectName: string | undefined = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, tableSymbol);
        if (!extObjectName)
            extObjectName = tableSymbol.name + ' Extension';
        extObjectName = await this.getObjectName("Please enter a name for the table extension.", extObjectName);
        if (!extObjectName) {
            return;
        }
        
        let relativeFileDir = await this.getRelativeFileDir(extObjType);
        await this.createAndShowNewTableExtension(tableSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
    }

    private async createAndShowNewTableExtension(tableSymbol: AZSymbolInformation, extObjType: AZSymbolKind, extObjectId: number, extObjectName: string, relativeFileDir: string | undefined) {
        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, tableSymbol);
        this.showNewDocument(this.buildTableExtForTable(tableSymbol, extObjectId, extObjectName), fileName, relativeFileDir);
    }

    //#endregion

    //#region Table Extension builders

    private buildTableExtForTable(tableSymbol : AZSymbolInformation, objectId : number, extObjectName : string) : string {
        
        let writer : ALSyntaxWriter = new ALSyntaxWriter(undefined);

        writer.writeStartExtensionObject("tableextension", objectId.toString(), extObjectName, tableSymbol.name);
        
        writer.writeStartFields();

        writer.writeLine("");
        
        writer.writeEndFields();

        writer.writeEndObject();
        
        return writer.toString();
    }

    //#endregion
}