import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from './objectBuilder';
import { relative } from 'path';

export class PageExtBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showPageExtWizard(pageSymbol : ALSymbolInfo) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const extObjType : ALSymbolKind = ALSymbolKind.PageExtension;
        
        let extObjectId : number = await this.getObjectId("Please enter an ID for the page extension.", 0);
        if (extObjectId < 0) {
            return;
        }

        let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, pageSymbol);
        extObjectName = await this.getObjectName("Please enter a name for the page extension.", extObjectName);
        if (!extObjectName) {
            return;
        }

        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, pageSymbol);
        let relativeFileDir: string = await this.getRelativeFileDir(extObjType);
        this.showNewDocument(this.buildPageExtForPage(pageSymbol, extObjectId, extObjectName), fileName, relativeFileDir);
    }

    //#endregion

    //#region Page Extension builders

    private buildPageExtForPage(pageSymbol : ALSymbolInfo, objectId : number, extObjectName : string) : string {
        
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartExtensionObject("pageextension", objectId, extObjectName, pageSymbol.symbolName);
        
        writer.writeStartLayout();

        writer.writeLine("");
        
        writer.writeEndLayout();
        
        writer.writeLine("");

        writer.writeStartActions();

        writer.writeLine("");

        writer.writeEndActions();

        writer.writeEndObject();
        
        return writer.toString();
    }

    //#endregion
}