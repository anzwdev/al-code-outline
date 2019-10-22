import * as vscode from 'vscode';
import { ALBaseAddFieldsCodeCommand } from "./alBaseAddFieldsCodeCommand";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALSyntaxWriter } from '../allanguage/alSyntaxWriter';
import { settings } from 'cluster';

export class ALAddXmlPortFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    elementType: string;
    
    constructor(context : DevToolsExtensionContext, newElementType: string) {
        super(context, 'AZDevTools.ALAddXmlPort' + newElementType + 'CodeCommand');
        this.elementType = newElementType;
    }

    protected async runAsync(range: vscode.Range) {
        //get required details from document source code
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        let isFieldSymbol = ((symbol.kind == AZSymbolKind.XmlPortFieldElement) || (symbol.kind == AZSymbolKind.XmlPortFieldAttribute));
        let dataItemSymbol = symbol;
        if (isFieldSymbol)
            dataItemSymbol = symbol.findParentByKind(AZSymbolKind.XmlPortTableElement);
        if ((!symbol) || 
            (!dataItemSymbol) ||
            (!dataItemSymbol.source) || 
            (!dataItemSymbol.contentRange) || 
            ((isFieldSymbol) && (!symbol.range)))            
            return;

        //get list of fields
        let fieldNames = await this._toolsExtensionContext.alLangProxy.getFieldList(this.getDocumentUri(), dataItemSymbol.source);

        //remove existing fields from the list
        fieldNames = this.removeExistingFields(fieldNames, dataItemSymbol.childSymbols, AZSymbolKind.QueryColumn, 'All available table fields have already been added to the query.');
        if (!fieldNames)
            return;

        //ask for fields
        let selectedFields = await vscode.window.showQuickPick(fieldNames, {
            canPickMany: true,
            placeHolder: 'Select table fields'
        });
        if (!selectedFields)
            return;

        let indent = dataItemSymbol.contentRange.start.character + 3;
    
        //insert fields
        let writer: ALSyntaxWriter = new ALSyntaxWriter();
        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            writer.writeNameSourceBlock(this.elementType, writer.createName(selectedFields[i]), dataItemSymbol.name + '.' + writer.encodeName(selectedFields[i]));
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }


}