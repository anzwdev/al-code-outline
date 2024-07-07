import * as vscode from 'vscode';
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { ALSyntaxWriter } from '../../allanguage/alSyntaxWriter';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { ALBaseAddFieldsCodeCommand } from './alBaseAddFieldsCodeCommand';
import { AZDocumentSymbolsLibrary } from '../../symbollibraries/azDocumentSymbolsLibrary';
import { ToolsGetPageDetailsRequest } from '../../langserver/symbolsinformation/toolsGetPageDetailsRequest';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';
import { TableFieldsSelector } from './tableFieldsSelector';
import { AppAreaMode } from '../../alsyntaxmodifiers/appAreaMode';
import { ToolsSymbolReference } from '../../langserver/symbolsinformation/toolsSymbolReference';
import { ALFieldToolTipsLocation } from '../../allanguage/alFieldToolTipsLocation';

export class ALAddPageFieldsCodeCommand extends ALBaseAddFieldsCodeCommand {
    constructor(context : DevToolsExtensionContext) {
        super(context, 'AddPageFields', 'AZDevTools.ALAddPageFieldsCodeCommand');
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, diagnostics: vscode.Diagnostic[], actions: vscode.CodeAction[]) {
        if ((symbol) && 
            ((symbol.kind == AZSymbolKind.PageGroup) ||                 
             (symbol.kind == AZSymbolKind.PageRepeater) ||
             (symbol.kind == AZSymbolKind.PageArea) ||
             (symbol.kind == AZSymbolKind.ControlAddChange) ||
             (symbol.kind == AZSymbolKind.PageField) ||
             (symbol.kind == AZSymbolKind.PageUserControl))) {                
            let action : vscode.CodeAction = new vscode.CodeAction("Add multiple fields (AZ AL Dev Tools)", vscode.CodeActionKind.QuickFix);
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
        if (!symbol) {
            return;
        }
        let config = vscode.workspace.getConfiguration('alOutline', document.uri);
        let parentKind: AZSymbolKind[] = [AZSymbolKind.PageObject, AZSymbolKind.PageExtensionObject];
        let pageSymbol = symbol.findParentByKindList(parentKind);
        let isFieldSymbol = ((symbol.kind === AZSymbolKind.PageField) || (symbol.kind === AZSymbolKind.PageUserControl));       
        let fieldToolTipsLocation = this._toolsExtensionContext.alLangProxy.fieldToolTipsLocation(document.uri);
        let addToolTips = (!!config.get<boolean>('addToolTipsToPageFields')) && (fieldToolTipsLocation === ALFieldToolTipsLocation.page);
        let useTableFieldCaptionsInApi = !!config.get<boolean>('useTableFieldCaptionsInApiFields');
        let reuseToolTips = !config.get<boolean>('doNotReuseToolTipsFromOtherPages');
        let toolTipsSource = config.get<string[]>('reuseToolTipsFromDependencies');

        if ((!pageSymbol) || 
            ((!isFieldSymbol) && (!symbol.contentRange)) || 
            ((isFieldSymbol) && (!symbol.range)) || 
            ((!pageSymbol.source) && (!pageSymbol.extends))) {
            return;
        }

        let isApiPage : boolean = ((!!pageSymbol.subtype) && (pageSymbol.subtype.toLowerCase() === 'api'));

        let pageReference: ToolsSymbolReference = (pageSymbol.kind === AZSymbolKind.PageExtensionObject)?{
            usings: pageSymbol.usings,
            nameWithNamespaceOrId: pageSymbol.extends
        }:{
            namespaceName: pageSymbol.namespaceName,
            name: pageSymbol.name,
            id: pageSymbol.id
        };

        //get available fields from the language server
        let response = await this. _toolsExtensionContext.toolsLangServerClient.getPageDetails(
            new ToolsGetPageDetailsRequest(document.uri.fsPath, pageReference, false, true, reuseToolTips, toolTipsSource));
        if ((!response) || (!response.symbol) || (!response.symbol.availableTableFields))
            return;

        let fieldNames: TableFieldInformation[] = response.symbol.availableTableFields;

        //ask for fields
        let fieldsSelector = new TableFieldsSelector(this._toolsExtensionContext);
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
        writer.applicationAreaMode = AppAreaMode.addToAllControls;

        if ((pageSymbol.kind == AZSymbolKind.PageObject) && (response.symbol.applicationArea) && (response.symbol.applicationArea != '')) {
            writer.applicationArea = response.symbol.applicationArea;            
            writer.applicationAreaMode = this._toolsExtensionContext.alLangProxy.getAppAreaMode(document.uri);
        }

        writer.setIndent(indent);
        for (let i=0; i<selectedFields.length; i++) {
            if (isApiPage)
                writer.writeApiPageField(selectedFields[i].name!, selectedFields[i].caption, 
                    selectedFields[i].captionLabel?.comment, useTableFieldCaptionsInApi);
            else
                writer.writePageField(selectedFields[i].name!, selectedFields[i].caption,
                    selectedFields[i].captionLabel?.comment, selectedFields[i].description, addToolTips, selectedFields[i].toolTips);
        }
        let source = writer.toString();

        await this.insertSymbolContentAsync(symbol, source);
    }

}