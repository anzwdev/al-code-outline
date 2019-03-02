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

    async showTableExtWizard(tableSymbol : ALSymbolInfo) {
        const extObjType : ALSymbolKind = ALSymbolKind.TableExtension;

        let extObjectId : number = await this.getObjectId("Please enter an ID for the table extension.", 0);

        let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, tableSymbol);
        extObjectName = await this.getObjectName("Please enter a name for the table extension.", extObjectName);
        if (!extObjectName) {
            return;
        }
        
        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, tableSymbol);
        this.showNewDocument(this.buildTableExtForTable(tableSymbol, extObjectId, extObjectName), fileName, extObjType);
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