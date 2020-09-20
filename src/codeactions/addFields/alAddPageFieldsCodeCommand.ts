import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { ALBaseAddFieldsCodeCommand } from './alBaseAddFieldsCodeCommand';
import { FieldsSelector } from './fieldsSelector';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';

export class ALAddPageFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AddPageFields', 'AZDevTools.ALAddPageFieldsCodeCommand');
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        if ((symbol) && 
            ((symbol.kind == AZSymbolKind.PageGroup) ||                 
             (symbol.kind == AZSymbolKind.PageRepeater) ||
             (symbol.kind == AZSymbolKind.PageArea) ||
             (symbol.kind == AZSymbolKind.ControlAddChange) ||
             (symbol.kind == AZSymbolKind.PageField) ||
             (symbol.kind == AZSymbolKind.PageUserControl))) {                
            let action : vscode.CodeAction = new vscode.CodeAction("Add multiple fields", vscode.CodeActionKind.QuickFix);
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
        let parentKind: AZSymbolKind[] = [AZSymbolKind.PageObject, AZSymbolKind.PageExtensionObject];
        let pageSymbol = symbol.findParentByKindList(parentKind);
        let isFieldSymbol = ((symbol.kind == AZSymbolKind.PageField) || (symbol.kind == AZSymbolKind.PageUserControl));

        if ((!pageSymbol) || 
            ((!isFieldSymbol) && (!symbol.contentRange)) || 
            ((isFieldSymbol) && (!symbol.range)) || 
            ((!pageSymbol.source) && (!pageSymbol.extends)))
            
            return;

        //collect existing page fields
        let existingFields : AZSymbolInformation[] = [];
        pageSymbol.collectChildSymbols(AZSymbolKind.PageField, true, existingFields);

        //load list of table fields
        let fieldNames: string[] | undefined;
        
        if ((pageSymbol.kind == AZSymbolKind.PageObject) && (pageSymbol.source))
            fieldNames = await this._toolsExtensionContext.alLangProxy.getFieldList(this.getDocumentUri(), pageSymbol.source);
        else if (pageSymbol.extends)
            fieldNames = await this._toolsExtensionContext.alLangProxy.getAvailablePageFieldList(this.getDocumentUri(), pageSymbol.extends);
        else
            return;

        //remove existing fields from the list
        fieldNames = this.removeExistingFields(fieldNames, existingFields, AZSymbolKind.PageField, 'All available table fields have already been added to the page.');
        if (!fieldNames)
            return;

        //ask for fields
        let fieldsSelector = new FieldsSelector();
        let selectedFields = await fieldsSelector.selectFields('Select table fields', fieldNames);
        if (!selectedFields)
            return;

        let indent = 0;
        let fieldsContainer: AZSymbolInformation | undefined = symbol;
        if (isFieldSymbol)
            fieldsContainer = symbol.parent;
        if ((fieldsContainer) && (fieldsContainer.contentRange))
            indent = fieldsContainer.contentRange.start.character + 3;
        else if (symbol.range)
            indent = symbol.range.start.character + 3;

        //insert fields
        let writer: ALSyntaxWriter = new ALSyntaxWriter(document.uri);
        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            writer.writePageField(selectedFields[i]);
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }



}