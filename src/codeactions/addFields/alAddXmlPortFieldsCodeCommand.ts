import * as vscode from 'vscode';
import { ALBaseAddFieldsCodeCommand } from "./alBaseAddFieldsCodeCommand";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { settings } from 'cluster';
import { FieldsSelector } from './fieldsSelector';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALAddXmlPortFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    elementType: string;
    commandTitle: string;
    
    constructor(context : DevToolsExtensionContext, newElementType: string, newCommandTitle: string) {
        super(context, 'AddXmlPort' + newElementType, 'AZDevTools.ALAddXmlPort' + newElementType + 'CodeCommand');
        this.elementType = newElementType;
        this.commandTitle = newCommandTitle;
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) &&         
            ((symbol.kind == AZSymbolKind.XmlPortTableElement) ||
             (symbol.kind == AZSymbolKind.XmlPortFieldElement) ||
             (symbol.kind == AZSymbolKind.XmlPortFieldAttribute))) {
            let action = new vscode.CodeAction(this.commandTitle, vscode.CodeActionKind.QuickFix);
            action.command = { 
                command: this.name, 
                title: this.commandTitle + '...',
                arguments: [docSymbols, document, range]
            };
            actions.push(action);
        }
    }

    protected async runAsync(docSymbols: AZDocumentSymbolsLibrary, document: vscode.TextDocument, range: vscode.Range) {
        //get required details from document source code
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if (!symbol)
            return;        
        let isFieldSymbol = ((symbol.kind == AZSymbolKind.XmlPortFieldElement) || (symbol.kind == AZSymbolKind.XmlPortFieldAttribute));
        let dataItemSymbol: AZSymbolInformation | undefined = symbol;
        if (isFieldSymbol)
            dataItemSymbol = symbol.findParentByKind(AZSymbolKind.XmlPortTableElement);
        if ((!dataItemSymbol) ||
            (!dataItemSymbol.source) || 
            (!dataItemSymbol.contentRange) || 
            ((isFieldSymbol) && (!symbol.range)))            
            return;

        //get list of fields
        let fieldNames: string[] | undefined = await this._toolsExtensionContext.alLangProxy.getFieldList(this.getDocumentUri(), dataItemSymbol.source);

        //remove existing fields from the list
        fieldNames = this.removeExistingFields(fieldNames, dataItemSymbol.childSymbols, AZSymbolKind.QueryColumn, 'All available table fields have already been added to the query.');
        if (!fieldNames)
            return;

        //ask for fields
        let fieldsSelector = new FieldsSelector();
        let selectedFields = await fieldsSelector.selectFields('Select table fields', fieldNames);
        if (!selectedFields)
            return;

        let indent = dataItemSymbol.contentRange.start.character + 3;
    
        //insert fields
        let writer: ALSyntaxWriter = new ALSyntaxWriter(document.uri);
        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            writer.writeNameSourceBlock(this.elementType, writer.createName(selectedFields[i]), dataItemSymbol.name + '.' + writer.encodeName(selectedFields[i]));
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }


}