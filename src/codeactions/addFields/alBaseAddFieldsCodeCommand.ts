import * as vscode from 'vscode';
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALCodeCommand } from '../alCodeCommand';
import { AZSymbolInformation } from '../../symbollibraries/azSymbolInformation';
import { AZSymbolKind } from '../../symbollibraries/azSymbolKind';

export class ALBaseAddFieldsCodeCommand extends ALCodeCommand {
    constructor(context : DevToolsExtensionContext, shortName: string, commandName: string) {
        super(context, shortName, commandName);
    }
    
    protected removeExistingFields(fieldNames: string[] | undefined, existingFields: AZSymbolInformation[] | undefined, existingFieldKind: AZSymbolKind, noFieldsMessage : string) : string[] | undefined {
        if (!fieldNames)
            return undefined;        
        if (existingFields) {        
            for (let i=0; i<existingFields.length; i++) {
                if ((existingFields[i].kind == existingFieldKind) && (existingFields[i].source)) {
                    let idx = fieldNames.indexOf(existingFields[i].source!)
                    if (idx >= 0) {
                        if (idx < (fieldNames.length - 1))
                            fieldNames[idx] = fieldNames[fieldNames.length - 1];
                        fieldNames.pop();
                    }
                }
            }
        }
        if (fieldNames.length == 0) {
            vscode.window.showWarningMessage(noFieldsMessage);
            return undefined;
        }
        return fieldNames.sort();
    }

    protected async insertSymbolContentAsync(symbol: AZSymbolInformation, content: string) {
        if (!vscode.window.activeTextEditor)
            return;
        
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
                content = '\n' + content; 
            }; 

        }

        await vscode.window.activeTextEditor.edit(editBuilder => {
            editBuilder.insert(new vscode.Position(line, column), content);
        });
    }

}