import * as vscode from 'vscode';
import { ALSyntaxWriter } from "../../allanguage/alSyntaxWriter";
import { ALPageWizardData } from "../wizards/alPageWizardData";

export class ALPageSyntaxBuilder {
    constructor() {      
    }    

    buildFromPageWizardData(destUri: vscode.Uri | undefined, data : ALPageWizardData) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(destUri);
        if (data.applicationArea)
            writer.applicationArea = data.applicationArea;

        let isApi : boolean = (data.pageType.toLowerCase() === "api");

        writer.writeStartObject("page", data.objectId, data.objectName);
        writer.writeLine("");
        writer.addProperty("PageType", data.pageType);
        writer.addProperty("SourceTable", writer.encodeName(data.selectedTable));
        
        if (isApi) {
            writer.addProperty("APIPublisher", writer.encodeString(data.apiPublisher));
            writer.addProperty("APIGroup", writer.encodeString(data.apiGroup));
            writer.addProperty("APIVersion", writer.encodeString(data.apiVersion));
            writer.addProperty("EntityName", writer.encodeString(data.entityName));
            writer.addProperty("EntitySetName", writer.encodeString(data.entitySetName));
            writer.addProperty("DelayedInsert", "true");
            writer.addProperty("Caption", writer.encodeString(writer.createApiName(data.objectName)));
        } else {
            writer.addProperty("Caption", writer.encodeString(data.objectName));
        }

        //usage category and application area for list pages
        if (data.pageType === "List") {            
            if ((data.usageCategory) && (data.usageCategory !== "")) {
                //application area requires useage category to be set
                if ((data.applicationArea) && (data.applicationArea !== ""))
                    writer.addProperty("ApplicationArea", data.applicationArea);
                writer.addProperty("UsageCategory", data.usageCategory);
            }
        }

        writer.writeProperties();

        writer.writeLine("");
        
        writer.writeStartLayout();
        writer.writeStartGroup("area", "content");
        
        if (data.isFastTabsPageType()) {
            if (data.fastTabsData) {
                for (let tabIdx = 0; tabIdx < data.fastTabsData.length; tabIdx++) {
                    let fastTab = data.fastTabsData[tabIdx];
                    writer.writeStartGroup("group", fastTab.name);
                    if (fastTab.fields) {
                        for (let fldIdx = 0; fldIdx < fastTab.fields.length; fldIdx++) {
                            writer.writePageField(fastTab.fields[fldIdx]);
                        }
                    }
                    writer.writeEndBlock();                    
                }
            }            
        } else {
            writer.writeStartGroup("repeater", "General");        
            if (data.selectedFieldList) {
                for (let i=0; i<data.selectedFieldList.length; i++) {
                    if (isApi)
                        writer.writeApiPageField(data.selectedFieldList[i]);                
                    else
                        writer.writePageField(data.selectedFieldList[i]);                
                }
            }       
            writer.writeEndBlock();
        }
        
        writer.writeEndBlock();
        
        writer.writeEndLayout();
        
        writer.writeLine("");
        writer.writeEndObject();
        
        return writer.toString();
    }

}