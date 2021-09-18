import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALCodeCommand } from '../alCodeCommand';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';
import { ALSyntaxHelper } from '../../allanguage/alSyntaxHelper';
import { TableFieldInformation } from '../../symbolsinformation/tableFieldInformation';
import { ToolsGetTableFieldsListRequest } from '../../langserver/symbolsinformation/toolsGetTableFieldsListRequest';

export class ALBaseAddFieldsCodeCommand extends ALCodeCommand {
    constructor(context : DevToolsExtensionContext, shortName: string, commandName: string) {
        super(context, shortName, commandName);
    }
    
    protected async getTableFields(name: string): Promise<TableFieldInformation[] | undefined> {
        let response = await this._toolsExtensionContext.toolsLangServerClient.getTableFieldsList(new ToolsGetTableFieldsListRequest(
            this.getDocumentUri()?.fsPath, name, false, false, true, true, true));
        if (!response)
            return;
        return response.symbols;
    }

    protected removeExistingFields(fields: TableFieldInformation[] | undefined, existingFields: AZSymbolInformation[] | undefined, existingFieldKind: AZSymbolKind, noFieldsMessage : string) : TableFieldInformation[] | undefined {
        if (!fields)
            return undefined;        
        if (existingFields) {        
            for (let i=0; i<existingFields.length; i++) {
                let srcName = existingFields[i].source;
                if ((existingFields[i].kind == existingFieldKind) && (srcName)) {
                    //detect fields declared as rec.
                    if (srcName.toLowerCase().startsWith("rec."))
                        srcName = ALSyntaxHelper.fromNameText(srcName.substr(4));

                    let idx = this.getFieldIndex(fields, srcName);
                    if (idx >= 0) {
                        if (idx < (fields.length - 1))
                            fields[idx] = fields[fields.length - 1];
                        fields.pop();
                    }
                }
            }
        }
        if (fields.length == 0) {
            vscode.window.showWarningMessage(noFieldsMessage);
            return undefined;
        }
        return this.sortFields(fields);
    }

    protected sortFields(fields: TableFieldInformation[]): TableFieldInformation[] {
        return fields.sort((a,b) => {
            if (a.name! > b.name!)
                return 1;
            if (a.name! < b.name!)
                return -1;
            return 0;
        });
    }

    protected getFieldIndex(fields: TableFieldInformation[], name: string) {
        name = name.toLowerCase();
        for (let i=0; i<fields.length; i++) {
            if ((fields[i].name) && (fields[i].name?.toLowerCase() == name))
                return i;
        }
        return -1;
    }

    protected async insertSymbolContentAsync(symbol: AZSymbolInformation, content: string) {
        if (!vscode.window.activeTextEditor)
            return;
        
        let eol = vscode.window.activeTextEditor.document.eol;

        let line : number = 0;
        let column : number = 0;

        if ((symbol.kind == AZSymbolKind.PageField) ||
            (symbol.kind == AZSymbolKind.PageUserControl) ||
            (symbol.kind == AZSymbolKind.QueryColumn) ||
            (symbol.kind == AZSymbolKind.ReportColumn) ||
            (symbol.kind == AZSymbolKind.XmlPortFieldElement) ||
            (symbol.kind == AZSymbolKind.XmlPortFieldAttribute)) {

            if (symbol.range) {
                line = symbol.range.end.line;
                column = symbol.range.end.character;
            }
        } else if (symbol.contentRange) {
            line = symbol.contentRange.end.line;
            let nextSymbolColumn : number = symbol.contentRange.end.character;

            if ((symbol.childSymbols) && (symbol.childSymbols.length > 0)) {            
                for (let i=0; i<symbol.childSymbols.length; i++) {
                    if ((symbol.childSymbols[i].kind !== AZSymbolKind.PropertyList) && (symbol.childSymbols[i].range) && (symbol.childSymbols[i].range!.start.line < line)) {
                        line = symbol.childSymbols[i].range!.start.line;
                        nextSymbolColumn = symbol.childSymbols[i].range!.start.character;
                    }
                }
            }
            
            //is insert in the first content line?
            if (line == symbol.contentRange.start.line) {
                column = nextSymbolColumn;
                content = eol + content; 
            }; 

        }

        await vscode.window.activeTextEditor.edit(editBuilder => {
            editBuilder.insert(new vscode.Position(line, column), content);
        });
    }

}