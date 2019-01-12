import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from '../alSymbolKind';
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from './objectBuilder';

export class PageExtBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showPageExtWizard(pageSymbol : ALSymbolInfo) {
        const extObjType : ALSymbolKind = ALSymbolKind.PageExtension;

        let extObjectIdString : string = "0";
        let promptForObjectId: boolean = vscode.workspace.getConfiguration('alOutline').get('promptForObjectId');
        if (promptForObjectId) {
            extObjectIdString = await this.getPageExtId(extObjectIdString);
        }

        if (!extObjectIdString) {
            extObjectIdString = "0";
        }
        
        let extObjectId : number = Number(extObjectIdString);
        if (isNaN(extObjectId)) {
            return;
        }

        let extObjectName: string = FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, pageSymbol);
        let promptForObjectName: boolean = vscode.workspace.getConfiguration('alOutline').get('promptForObjectName');
        if (promptForObjectName) {
            extObjectName = await this.getPageExtName(extObjectName);
        }
        
        if (!extObjectName) {
            return;
        }

        let stripChars: string = vscode.workspace.getConfiguration('alOutline').get('stripNonAlphanumericCharactersFromObjectNames');
        if (stripChars) {
            extObjectName = FileBuilder.stripNonAlphaNumericCharacters(extObjectName);
        }

        let fileName : string = FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, pageSymbol);
        this.showNewDocument(this.buildPageExtForPage(pageSymbol, extObjectId, extObjectName), fileName, extObjType);
    }

    //#endregion

    //#region UI functions

    private getPageExtId(defaultExtName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultExtName,
            prompt : "Please enter an id for the page extension."
        });
    }

    private getPageExtName(defaultExtName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultExtName,
            prompt : "Please enter a name for the page extension."
        });
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