import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALBaseAddFieldsCodeCommand } from './alBaseAddFieldsCodeCommand';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { TableFieldsSelector } from './tableFieldsSelector';
import { ToolsGetQueryDataItemDetailsRequest } from '../../langserver/symbolsinformation/toolsGetQueryDataItemDetailsRequest';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';

export class ALAddQueryFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AddQueryFields', 'AZDevTools.ALAddQueryFieldsCodeCommand');
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {        
        if ((symbol) && 
            ((symbol.kind == AZSymbolKind.QueryDataItem) ||
             (symbol.kind == AZSymbolKind.QueryColumn))) {
            let action = new vscode.CodeAction("Add multiple fields (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
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
        let isFieldSymbol = (symbol.kind == AZSymbolKind.QueryColumn);
        let dataItemSymbol: AZSymbolInformation | undefined = symbol;
        if (isFieldSymbol)
            dataItemSymbol = symbol.findParentByKind(AZSymbolKind.QueryDataItem);
        if ((!symbol) || 
            (!dataItemSymbol) ||
            (!dataItemSymbol.source) || 
            (!dataItemSymbol.contentRange) || 
            ((isFieldSymbol) && (!symbol.range)))            
            return;

        let objectSymbol = dataItemSymbol.findParentByKind(AZSymbolKind.QueryObject);
        if (!objectSymbol)
            return;
        let isApi: boolean = ((!!objectSymbol.subtype) && (objectSymbol.subtype.toLowerCase() == 'api'));

        //get list of fields
        let response = await this._toolsExtensionContext.toolsLangServerClient.getQueryDataItemDetails(
            new ToolsGetQueryDataItemDetailsRequest(document.uri.fsPath, {
                namespaceName: objectSymbol.namespaceName,
                name: objectSymbol.name,
                id: objectSymbol.id
            }, 
            dataItemSymbol.name, false, true));
        if ((!response) || (!response.symbol) || (!response.symbol.availableTableFields))
            return;

        let fields: TableFieldInformation[] = response.symbol.availableTableFields;

        /*
        //get list of fields
        let fields = await this.getTableFields(dataItemSymbol.source);

        //remove existing fields from the list
        fields = this.removeExistingFields(fields, dataItemSymbol.childSymbols, AZSymbolKind.QueryColumn, 'All available table fields have already been added to the query.');
        if (!fields)
            return;
        */

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
            let columnName = isApi?writer.createApiName(selectedFields[i].name!):writer.createName(selectedFields[i].name!);
            writer.writeNameSourceBlock("column", writer.encodeName(columnName), writer.encodeName(selectedFields[i].name!));
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source, range);
    }
}