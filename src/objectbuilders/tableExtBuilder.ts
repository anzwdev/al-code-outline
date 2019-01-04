import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ObjectBuilder } from './objectBuilder';

export class TableExtBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showTableExtWizard(tableSymbol : ALSymbolInfo) {
        let extObjectIdString : string = await this.getTableExtId(
            "0"
        );

        if (!extObjectIdString) {
            extObjectIdString = "0";
        }
        
        let extObjectId : number = Number(extObjectIdString);
        if (isNaN(extObjectId)) {
            return;
        }

        let extObjectName : string = await this.getTableExtName(
            tableSymbol.symbolName.replace(/ /g,'') + "Ext"
        );
        
        if (!extObjectName) {
            return;
        }

        this.showNewDocument(this.buildTableExtForTable(tableSymbol, extObjectId, extObjectName));
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