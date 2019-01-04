import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ObjectBuilder } from './objectBuilder';

export class PageExtBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showPageExtWizard(pageSymbol : ALSymbolInfo) {
        let extObjectIdString : string = await this.getPageExtId(
            "0"
        );

        if (!extObjectIdString) {
            extObjectIdString = "0";
        }
        
        let extObjectId : number = Number(extObjectIdString);
        if (isNaN(extObjectId)) {
            return;
        }

        let extObjectName : string = await this.getPageExtName(
            pageSymbol.symbolName.replace(/ /g,'') + "Ext"
        );
        
        if (!extObjectName) {
            return;
        }

        this.showNewDocument(this.buildPageExtForPage(pageSymbol, extObjectId, extObjectName));
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