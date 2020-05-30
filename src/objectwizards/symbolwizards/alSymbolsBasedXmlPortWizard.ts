import * as vscode from 'vscode';
import { FileBuilder } from '../fileBuilder';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSymbolsBasedWizard } from './alSymbolsBasedWizard';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';

export class ALSymbolsBasedXmlPortWizard extends ALSymbolsBasedWizard {

    constructor() {
        super();
    }

    //#region Wizards with UI

    async showWizard(tableSymbols: AZSymbolInformation[]) {
        if (tableSymbols.length == 1)
            await this.showXmlPortWizard(tableSymbols[0]);
        else
            await this.showMultiXmlPortWizard(tableSymbols);
    }

    async showMultiXmlPortWizard(tableSymbols: AZSymbolInformation[]) {
        if (!FileBuilder.checkCrsExtensionFileNamePatternRequired())
            return;

        const objType : AZSymbolKind = AZSymbolKind.XmlPortObject;

        let startObjectId: number = await this.getObjectId(`Please enter a starting ID for the xmlport objects.`, 0);
        if (startObjectId < 0) {
            return;
        }

        let fieldsAsElements: boolean | undefined = await this.promptForFieldsAsElements();
        if (fieldsAsElements === undefined) {
            return;
        }
        let relativeFileDir = await this.getRelativeFileDir(objType);

        for (let i = 0; i < tableSymbols.length; i++) {
            let tableSymbol = tableSymbols[i];
            let objectId: number = startObjectId + i;
            let objectName : string = this.getDefaultXmlPortName(tableSymbol);

            await this.createAndShowNewXmlPort(tableSymbol, objType, objectId, objectName, fieldsAsElements, relativeFileDir);
        }
    }

    async showXmlPortWizard(tableSymbol : AZSymbolInformation) {
        if (!FileBuilder.checkCrsFileNamePatternRequired())
            return;
            
        const objType : AZSymbolKind = AZSymbolKind.XmlPortObject;

        let objectId : number = await this.getObjectId("Please enter an ID for the xmlport object.", 0);
        if (objectId < 0) {
            return;
        }

        let objectName = await this.getObjectName("Please enter a name for the xmlport object.", this.getDefaultXmlPortName(tableSymbol));
        if (!objectName)
            return;

        let fieldsAsElements: boolean | undefined = await this.promptForFieldsAsElements();
        if (!fieldsAsElements) {
            return;
        }
        
        let relativeFileDir = await this.getRelativeFileDir(objType);
        await this.createAndShowNewXmlPort(tableSymbol, objType, objectId, objectName, fieldsAsElements, relativeFileDir);
    }

    private async createAndShowNewXmlPort(tableSymbol: AZSymbolInformation, objType: AZSymbolKind, objectId: number, objectName: string, fieldsAsElements: boolean, relativeFileDir: string | undefined) {
        let fileName : string = await FileBuilder.getPatternGeneratedFullObjectFileName(objType, objectId, objectName);
        this.showNewDocument(this.buildXmlPortForTable(tableSymbol, objectId, objectName, fieldsAsElements), fileName, relativeFileDir);
    }

    //#endregion
  
    //#region Report builders

    buildXmlPortForTable(tableSymbol : AZSymbolInformation, objectId : number, objectName : string, fieldsAsElements : boolean) : string {
        //generate file content
        let writer : ALSyntaxWriter = new ALSyntaxWriter(undefined);

        writer.writeStartObject("xmlport", objectId.toString(), objectName);

        //write dataset
        this.appendSchema(writer, tableSymbol, fieldsAsElements);

        //write report request page suggetsion
        this.appendReportRequestPage(writer);

        writer.writeEndObject();
        
        return writer.toString();
    }

    private appendSchema(writer : ALSyntaxWriter, tableSymbol : AZSymbolInformation, fieldsAsElements : boolean) {       
        var tableElementName = writer.createName(tableSymbol.name);
        var fieldNodeName : string;
        if (fieldsAsElements)
            fieldNodeName = "fieldelement";
        else
            fieldNodeName = "fieldattribute";

        writer.writeStartNamedBlock("schema");
        writer.writeStartGroup("textelement", "RootNodeName");

        writer.writeStartNameSourceBlock("tableelement", tableElementName, writer.encodeName(tableSymbol.name));

        let fieldList : AZSymbolInformation[] = [];
        tableSymbol.collectChildSymbols(AZSymbolKind.Field, true, fieldList);
        fieldList.forEach(
            item => {
                writer.writeNameSourceBlock(fieldNodeName, writer.createName(item.name), tableElementName + "." + writer.encodeName(item.name));
            }
        );

        writer.writeEndBlock();

        writer.writeEndBlock();
        writer.writeEndBlock();
    }

    private appendReportRequestPage(writer : ALSyntaxWriter) {
        writer.writeStartNamedBlock("requestpage");

        //layout
        writer.writeStartLayout();
        writer.writeStartGroup("area", "content");
        writer.writeStartGroup("group", "GroupName");
        writer.writeEndBlock();
        writer.writeEndBlock();
        writer.writeEndLayout();

        //actions
        writer.writeStartNamedBlock("actions")
        writer.writeStartGroup("area", "processing");
        writer.writeEndBlock();
        writer.writeEndBlock();

        writer.writeEndBlock();
    }

    //#endregion

    //#region Helper Methods

    private async promptForFieldsAsElements() : Promise<boolean | undefined> {
        let fieldAsAttributeText = "Table fields as xml attributes";
        let fieldAsElementText = "Table fields as xml elements";

        let fieldNodeTypes = [
            { 
                label: fieldAsAttributeText, 
                description: ""
            },
            {
                label : fieldAsElementText,
                description : ""
            }];

        let selectedNodeType = await vscode.window.showQuickPick(fieldNodeTypes);
        if ((!selectedNodeType) || (!selectedNodeType.label)) {
            return undefined;
        }
        
        return (selectedNodeType.label == fieldAsElementText);
    }

    private getDefaultXmlPortName(tableSymbol: AZSymbolInformation) : string {
        return `${tableSymbol.name.trim()} XmlPort`;
    }
    
    //#endregion
}