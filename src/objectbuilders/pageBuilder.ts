import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from "../alSymbolKind";
import { FileBuilder } from './fileBuilder';
import { ObjectBuilder } from './objectBuilder';

export class PageBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showListPageWizard(tableSymbol : ALSymbolInfo) {
        const objType : ALSymbolKind = ALSymbolKind.Page;

        let objectId : number = await this.getObjectId("Please enter an ID for the list page.", 0);

        let objectName : string = tableSymbol.symbolName.trim() + " List";
        objectName = await this.getObjectName("Please enter a name for the list page.", objectName);
        
        if (!objectName) {
            return;
        }

        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildListPageForTable(tableSymbol, objectId, objectName), fileName, objType);
    }

    async showCardPageWizard(tableSymbol : ALSymbolInfo) {
        const objType : ALSymbolKind = ALSymbolKind.Page;

        let objectId : number = await this.getObjectId("Please enter an ID for the card page.", 0);

        let objectName : string = tableSymbol.symbolName.trim() + " Card";
        objectName = await this.getObjectName("Please enter a name for the card page.", objectName);
        
        if (!objectName) {
            return;
        }

        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildCardPageForTable(tableSymbol, objectId, objectName), fileName, objType);
    }

    //#endregion

    //#region Page builders

    buildListPageForTable(tableSymbol : ALSymbolInfo, objectId : number, objectName : string) : string {
        return this.buildPageForTable(tableSymbol, objectId, objectName, "List", "repeater");
    }
    
    buildCardPageForTable(tableSymbol : ALSymbolInfo, objectId : number, objectName : string) : string {
        return this.buildPageForTable(tableSymbol, objectId, objectName, "Card", "group");
    }

    private buildPageForTable(tableSymbol : ALSymbolInfo, objectId : number, objectName : string, pageType : string, fieldGroupType : string) : string {
       
        //generate file content
        let writer : ALObjectWriter = new ALObjectWriter();

        writer.writeStartObject("page", objectId, objectName);
        writer.writeLine("");
        writer.writeProperty("PageType", pageType);
        writer.writeProperty("SourceTable", writer.encodeName(tableSymbol.symbolName));
        writer.writeProperty("Caption", writer.encodeString(objectName));

        //usage category and application area for list pages
        if (pageType === "List") {
            let appArea : string = vscode.workspace.getConfiguration('alOutline').get('defaultAppArea');
            let usageCategory : string = vscode.workspace.getConfiguration('alOutline').get('defaultListUsageCategory');
            
            if ((usageCategory) && (usageCategory !== "")) {
                //application area requires useage category to be set
                if ((appArea) && (appArea !== ""))
                    writer.writeProperty("ApplicationArea", appArea);
                writer.writeProperty("UsageCategory", usageCategory);
            }
        }

        writer.writeLine("");
        
        writer.writeStartLayout();
        writer.writeStartGroup("area", "content");
        
        writer.writeStartGroup(fieldGroupType, "General");
        
        let fieldList : ALSymbolInfo[] = [];
        tableSymbol.getAllSymbolsByKind(ALSymbolKind.Field, fieldList);
        fieldList.forEach(
            item => {
                writer.writePageField(item.symbolName);
            }
        );
        
        writer.writeEndBlock();
        
        writer.writeEndBlock();
        
        writer.writeEndLayout();
        
        writer.writeLine("");
        writer.writeEndObject();
        
        return writer.toString();
    }

    //#endregion

}