import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALSyntaxWriter } from '../allanguage/alSyntaxWriter';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../symbollibraries/azSymbolInformation';
import { ALBaseAddFieldsCodeCommand } from './alBaseAddFieldsCodeCommand';

export class ALAddPageFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALAddPageFieldsCodeCommand');
    }

    protected async runAsync(range: vscode.Range) {
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
        pageSymbol.collectChildSymbols(AZSymbolKind.PageField, existingFields);

        //load list of table fields
        let fieldNames: string[] | undefined;
        
        if (pageSymbol.kind == AZSymbolKind.PageObject)        
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
        let selectedFields = await vscode.window.showQuickPick(fieldNames, {
            canPickMany: true,
            placeHolder: 'Select table fields'
        });
        if (!selectedFields)
            return;

        let indent = 0;
        let fieldsContainer = symbol;
        if (isFieldSymbol)
            fieldsContainer = symbol.parent;
        if ((fieldsContainer) && (fieldsContainer.contentRange))
            indent = fieldsContainer.contentRange.start.character + 3;
        else
            indent = symbol.range.start.character + 3;

        //insert fields
        let writer: ALSyntaxWriter = new ALSyntaxWriter();
        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            writer.writePageField(selectedFields[i]);
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }



}