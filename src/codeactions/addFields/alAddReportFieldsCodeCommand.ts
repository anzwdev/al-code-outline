import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALBaseAddFieldsCodeCommand } from './alBaseAddFieldsCodeCommand';
import { FieldsSelector } from './fieldsSelector';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALAddReportFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AddReportFields', 'AZDevTools.ALAddReportFieldsCodeCommand');
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) && 
            ((symbol.kind == AZSymbolKind.ReportDataItem) ||
             (symbol.kind == AZSymbolKind.ReportColumn))) {
            let action = new vscode.CodeAction("Add multiple fields", vscode.CodeActionKind.QuickFix);
            action.command = { 
                command: this.name, 
                title: 'Add multiple fields...',
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
        let isFieldSymbol = (symbol.kind == AZSymbolKind.ReportColumn);
        let dataItemSymbol: AZSymbolInformation | undefined = symbol;
        if (isFieldSymbol)
            dataItemSymbol = symbol.findParentByKind(AZSymbolKind.ReportDataItem);
        if ((!dataItemSymbol) ||
            (!dataItemSymbol.source) || 
            (!dataItemSymbol.contentRange) || 
            ((isFieldSymbol) && (!symbol.range)))            
            return;

        //get list of fields
        let fieldNames: string[] | undefined = await this._toolsExtensionContext.alLangProxy.getFieldList(this.getDocumentUri(), dataItemSymbol.source);
        
        //remove existing fields from the list
        fieldNames = this.removeExistingFields(fieldNames, dataItemSymbol.childSymbols, AZSymbolKind.ReportColumn, 'All available table fields have already been added to the report.');
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
            writer.writeNameSourceBlock("column", writer.createName(selectedFields[i]), writer.encodeName(selectedFields[i]));
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }

}
