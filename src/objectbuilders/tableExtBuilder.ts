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

        let extObjectIdString : string = "0";
        let promptForObjectId: boolean = vscode.workspace.getConfiguration('alOutline').get('promptForObjectId');
        if (promptForObjectId) {
            extObjectIdString = await this.getTableExtId(extObjectIdString);
        }

        if (!extObjectIdString) {
            extObjectIdString = "0";
        }
        
        let extObjectId : number = Number(extObjectIdString);
        if (isNaN(extObjectId)) {
            return;
        }

        let extObjectName: string = FileBuilder.getPatternGeneratedExtensionObjectName(extObjType, extObjectId, tableSymbol);
        let promptForObjectName: boolean = vscode.workspace.getConfiguration('alOutline').get('promptForObjectName');
        if (promptForObjectName) {
            extObjectName = await this.getTableExtName(extObjectName);
        }
        
        if (!extObjectName) {
            return;
        }

        let stripChars: string = vscode.workspace.getConfiguration('alOutline').get('stripNonAlphanumericCharactersFromObjectNames');
        if (stripChars) {
            extObjectName = FileBuilder.stripNonAlphaNumericCharacters(extObjectName);
        }
        
        let fileName : string = FileBuilder.getPatternGeneratedExtensionObjectFileName(extObjType, extObjectId, extObjectName, tableSymbol);
        this.showNewDocument(this.buildTableExtForTable(tableSymbol, extObjectId, extObjectName), fileName, extObjType);
    }

    //#endregion

    //#region UI functions

    private getTableExtId(defaultExtName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultExtName,
            prompt : "Please enter an id for the table extension."
        });
    }

    private getTableExtName(defaultExtName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultExtName,
            prompt : "Please enter a name for the table extension."
        });
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