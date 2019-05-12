import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ALSyntaxWriter } from '../allanguage/alSyntaxWriter';
import { AZSymbolKind } from '../symbollibraries/azSymbolKind';
import { ALBaseAddFieldsCodeCommand } from './alBaseAddFieldsCodeCommand';

export class ALAddQueryFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AZDevTools.ALAddQueryFieldsCodeCommand');
    }

    protected async runAsync(range: vscode.Range) {
        //get required details from document source code
        let symbol = this._toolsExtensionContext.activeDocumentSymbols.findSymbolInRange(range);
        if ((!symbol) || (!symbol.source) || (!symbol.contentRange))
            return;

        //get list of fields
        let fieldNames = await this._toolsExtensionContext.alLangProxy.getFieldList(symbol.source);

        //remove existing fields from the list
        fieldNames = this.removeExistingFields(fieldNames, symbol.childSymbols, AZSymbolKind.QueryColumn, 'All available table fields have already been added to the query.');
        if (!fieldNames)
            return;

        //ask for fields
        let selectedFields = await vscode.window.showQuickPick(fieldNames, {
            canPickMany: true,
            placeHolder: 'Select table fields'
        });
        if (!selectedFields)
            return;

        let indent = symbol.contentRange.start.character + 3;
    
        //insert fields
        let writer: ALSyntaxWriter = new ALSyntaxWriter();
        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            writer.writeNameSourceBlock("column", writer.createName(selectedFields[i]), writer.encodeName(selectedFields[i]));
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }
}