import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from './objectBuilder';
import { relative } from 'path';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';

export class PageExtBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showWizard(tableSymbols: AZSymbolInformation[]) {
        if (tableSymbols.length == 1)
            await this.showPageExtWizard(tableSymbols[0]);
        else
            await this.showMultiPageExtWizard(tableSymbols);
    }

    async showMultiPageExtWizard(pageSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(true))
            return;

        const extObjType : AZSymbolKind = AZSymbolKind.PageExtensionObject;

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

    async showPageExtWizard(pageSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired() || !FileBuilder.checkCrsExtensionObjectNamePatternRequired(false))
            return;

        const extObjType : AZSymbolKind = AZSymbolKind.PageExtensionObject;
        
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

    private async createAndShowNewPageExtension(pageSymbol: AZSymbolInformation, extObjType: AZSymbolKind, extObjectId: number, extObjectName: string, relativeFileDir: string) {
        let fileName : string = await FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, pageSymbol);
        this.showNewDocument(this.buildPageExtForPage(pageSymbol, extObjectId, extObjectName), fileName, relativeFileDir);
    }

    //#endregion

    //#region Page Extension builders

    private buildPageExtForPage(pageSymbol : AZSymbolInformation, objectId : number, extObjectName : string) : string {
        
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartExtensionObject("pageextension", objectId, extObjectName, pageSymbol.name);
        
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