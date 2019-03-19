import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from './objectBuilder';

export class TableExtBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showMultiTableExtWizard(tableSymbols: ALSymbolInfo[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(true))
            return;

        const extObjType : ALSymbolKind = ALSymbolKind.TableExtension;

        let startObjectId: number = await this.getObjectId("Please enter a starting ID for the table extensions.", 0);
        if (startObjectId < 0) {
            return;
        }

        let relativeFileDir: string = await this.getRelativeFileDir(extObjType);

        for (let i = 0; i < tableSymbols.length; i++) {
            let tableSymbol = tableSymbols[i];
            let extObjectId: number = startObjectId + i;
            let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, tableSymbol);

            await this.createAndShowNewTableExtension(tableSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
        }
    }

    async showTableExtWizard(tableSymbol : ALSymbolInfo) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(false))
            return;
            
        const extObjType : ALSymbolKind = ALSymbolKind.TableExtension;

        let extObjectId : number = await this.getObjectId("Please enter an ID for the table extension.", 0);
        if (extObjectId < 0) {
            return;
        }

        let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, tableSymbol);
        extObjectName = await this.getObjectName("Please enter a name for the table extension.", extObjectName);
        if (!extObjectName) {
            return;
        }
        
        let relativeFileDir: string = await this.getRelativeFileDir(extObjType);
        await this.createAndShowNewTableExtension(tableSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
    }

    private async createAndShowNewTableExtension(tableSymbol: ALSymbolInfo, extObjType: ALSymbolKind, extObjectId: number, extObjectName: string, relativeFileDir: string) {
        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, tableSymbol);
        this.showNewDocument(this.buildTableExtForTable(tableSymbol, extObjectId, extObjectName), fileName, relativeFileDir);
    }

    //#endregion

    //#region Table Extension builders

    private buildTableExtForTable(tableSymbol : ALSymbolInfo, objectId : number, extObjectName : string) : string {
        
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartExtensionObject("tableextension", objectId, extObjectName, tableSymbol.symbolName);
        
        writer.writeStartFields();

        writer.writeLine("");
        
        writer.writeEndFields();

        writer.writeEndObject();
        
        return writer.toString();
    }

    //#endregion
}