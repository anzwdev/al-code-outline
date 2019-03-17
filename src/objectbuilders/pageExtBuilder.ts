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

    async showMultiPageExtWizard(pageSymbols: ALSymbolInfo[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const extObjType : ALSymbolKind = ALSymbolKind.PageExtension;

        let startObjectId: number = await this.getObjectId("Please enter a starting ID for the page extensions.", 0);
        if (startObjectId < 0) {
            return;
        }

        let relativeFileDir: string = await this.getRelativeFileDir(extObjType);

        for (let i = 0; i < pageSymbols.length; i++) {
            let pageSymbol = pageSymbols[i];
            let extObjectId: number = startObjectId + i;
            let extObjectName: string = await FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, pageSymbol);

            await this.createAndShowNewPageExtension(pageSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
        }
    }

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

        let relativeFileDir: string = await this.getRelativeFileDir(extObjType);
        await this.createAndShowNewPageExtension(pageSymbol, extObjType, extObjectId, extObjectName, relativeFileDir);
    }

    private async createAndShowNewPageExtension(pageSymbol: ALSymbolInfo, extObjType: ALSymbolKind, extObjectId: number, extObjectName: string, relativeFileDir: string) {
        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, pageSymbol);
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