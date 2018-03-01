import * as vscode from 'vscode';
import { ALObjectWriter } from "./alObjectWriter";
import { ALSymbolInfo } from "../alSymbolInfo";
import { ALSymbolKind } from "../alSymbolKind";
import { ObjectBuilder } from './objectBuilder';

export class PageBuilder extends ObjectBuilder {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showListPageWizard(tableSymbol : ALSymbolInfo) {
        let objectName : string = await this.getPageName(
            tableSymbol.symbolName.trim() + " List");
        
        if (!objectName)
            return;

        this.showNewDocument(this.buildListPageForTable(tableSymbol, 0, objectName));
    }

    async showCardPageWizard(tableSymbol : ALSymbolInfo) {
        let objectName : string = await this.getPageName(
            tableSymbol.symbolName.trim() + " Card");
        
        if (!objectName)
            return;

        this.showNewDocument(this.buildCardPageForTable(tableSymbol, 0, objectName));
    }

    //#endregion

    //#region UI functions

    private getPageName(defaultPageName : string) : Thenable<string> {
        return vscode.window.showInputBox({
            value : defaultPageName,
            prompt : "Please enter new page name"});
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
        
        tableSymbol.childItems.forEach(
            item => {
                if (item.alKind == ALSymbolKind.Field) {
                    writer.writePageField(item.symbolName);
                }
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