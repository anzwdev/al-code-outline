import * as vscode from 'vscode';
import { ALBaseAddFieldsCodeCommand } from "./alBaseAddFieldsCodeCommand";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { TableFieldsSelector } from './tableFieldsSelector';
import { ToolsGetXmlPortTableElementDetailsRequest } from '../../langserver/symbolsinformation/toolsGetXmlPortTableElementDetailsRequest';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';

export class ALAddXmlPortFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    elementType: string;
    commandTitle: string;
    
    constructor(context : DevToolsExtensionContext, newElementType: string, newCommandTitle: string) {
        super(context, 'AddXmlPort' + newElementType, 'AZDevTools.ALAddXmlPort' + newElementType + 'CodeCommand');
        this.elementType = newElementType;
        this.commandTitle = newCommandTitle;
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
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

        let xmlPortSymbol = dataItemSymbol.findParentByKind(AZSymbolKind.XmlPortObject);
        if (!xmlPortSymbol)
            return;

        //get list of fields
        let response = await this._toolsExtensionContext.toolsLangServerClient.getXmlPortTableElementDetails(
            new ToolsGetXmlPortTableElementDetailsRequest(document.uri.fsPath, {
                namespaceName: xmlPortSymbol.namespaceName,
                name: xmlPortSymbol.name,
                id: xmlPortSymbol.id
            }, dataItemSymbol.name, false, true));
        if ((!response) || (!response.symbol) || (!response.symbol.availableTableFields))
            return;

        let fields: TableFieldInformation[] = response.symbol.availableTableFields;

        //ask for fields
        let fieldsSelector = new TableFieldsSelector(this._toolsExtensionContext);
        let selectedFields = await fieldsSelector.selectFields('Select table fields', fields);
        if (!selectedFields)
            return;

        let indent = dataItemSymbol.contentRange.start.character + 3;
    
        //insert fields
        let writer: ALSyntaxWriter = new ALSyntaxWriter(document.uri);
        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            writer.writeNameSourceBlock(this.elementType, writer.createName(selectedFields[i].name!), dataItemSymbol.name + '.' + writer.encodeName(selectedFields[i].name!));
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }


}