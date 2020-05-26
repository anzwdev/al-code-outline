import * as vscode from 'vscode';
import { ALBaseSortCodeCommand } from "./alBaseSortCodeCommand";
import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { AZDocumentSymbolsLibrary } from "../../symbollibraries/azDocumentSymbolsLibrary";
import { AZSymbolInformation } from "../../symbollibraries/azSymbolInformation";
import { AZSymbolKind } from "../../symbollibraries/azSymbolKind";

export class ALSortVariablesCommand extends ALBaseSortCodeCommand {
    protected _typeByPriority: string[] = [
        "record ", "report", "codeunit", "xmlport", "page", "query", "notification", 
        "bigtext", "dateformula", "recordid", "recordref", "fieldref", "filterpagebuilder"];

    constructor(context : DevToolsExtensionContext) {
        super(context, "SortVariables");
    }

    collectCodeActions(docSymbols: AZDocumentSymbolsLibrary, symbol: AZSymbolInformation | undefined, document: vscode.TextDocument, range: vscode.Range | vscode.Selection, context: vscode.CodeActionContext, actions: vscode.CodeAction[]) {
        let edit: vscode.WorkspaceEdit | undefined = undefined;
        let fixOnSave = this.canRunOnSave(document.uri); 
        let actionKind = this.getCodeActionKind(fixOnSave);

        if (fixOnSave) {
            let objList: AZSymbolInformation[] = [];       
            if (docSymbols.rootSymbol) {
                docSymbols.rootSymbol.collectObjectSymbols(objList);
                for (let i=0; i<objList.length; i++)
                    edit = this.prepareEdit(objList[i], document, edit);
            }
        } else {
            //collect list of objects in selection range
            if ((symbol) && 
                ((symbol.isALObject()) || (symbol.kind == AZSymbolKind.VarSection) || (symbol.kind == AZSymbolKind.GlobalVarSection)) && 
                (symbol.selectionRange) &&
                (symbol.selectionRange.start.line == range.start.line))               
                edit = this.prepareEdit(symbol, document, undefined);
        }

        if (edit) {
            let action = new vscode.CodeAction("Sort variables", actionKind);
            action.edit = edit;
            actions.push(action);
        }
    }

    protected prepareEdit(symbol: AZSymbolInformation, document: vscode.TextDocument, edit: vscode.WorkspaceEdit | undefined): vscode.WorkspaceEdit | undefined {
        //collect VariableSection symbols
        let symbolsList: AZSymbolInformation[] = [];
        if ((symbol.kind == AZSymbolKind.VarSection) || (symbol.kind == AZSymbolKind.GlobalVarSection))
            symbolsList.push(symbol);
        else
            symbol.collectChildSymbolsByKindList([AZSymbolKind.VarSection,
                AZSymbolKind.GlobalVarSection], true, symbolsList);
 
        if (symbolsList.length == 0)
            return edit;

        if (!edit)
            edit = new vscode.WorkspaceEdit();
        if (symbolsList.length > 0) {
            for (let i=0; i<symbolsList.length; i++) {
                this.sortVariables(document, symbolsList[i], edit);
            }
        }
        return edit;
    }

    protected sortVariables(document: vscode.TextDocument, symbol: AZSymbolInformation, editBuilder: vscode.WorkspaceEdit) {
        // Collect nodes
        let childSymbolsList: AZSymbolInformation[] = [];
        symbol.collectChildSymbolsByKindList([AZSymbolKind.VariableDeclaration, 
            AZSymbolKind.VariableDeclarationName], false, childSymbolsList);
        if (childSymbolsList.length == 0)
            return;

        // Sort nodes
        childSymbolsList.sort((symbolA, symbolB) => {
            return this.compareSymbols(symbolA, symbolB);
        });

        // Produce the new sorted source
        let newSource: string = "";

        for (let i=0; i<childSymbolsList.length; i++) {
            let childSymbol = childSymbolsList[i];
            if (childSymbol.range) {
                //get symbol range
                let declRange = new vscode.Range(childSymbol.range.start.line, childSymbol.range.start.character, 
                    childSymbol.range.end.line, childSymbol.range.end.character);
            
                //build new source code
                newSource += document.getText(declRange);
                if (childSymbol.kind == AZSymbolKind.VariableDeclarationName) {
                    if ((i < (childSymbolsList.length - 1)) &&
                        (childSymbolsList[i+1].kind == childSymbol.kind) &&
                        (childSymbolsList[i+1].subtype == childSymbol.subtype)) {
                        newSource += ', ';
                        if (childSymbolsList[i+1].range!.start.line < childSymbolsList[i+1].range!.end.line)
                            newSource += '\n';
                    } else
                        newSource += ': ' + childSymbol.subtype + ';\n';
                }
            }
        }
        
        // Delete the old unsorted columns and insert the new sorted source
        if (symbol.contentRange) {
            const deleteRange = new vscode.Range(symbol.contentRange.start.line, symbol.contentRange.start.character, 
                symbol.contentRange.end.line, symbol.contentRange.end.character);
            editBuilder.delete(document.uri, deleteRange);
            
            editBuilder.insert(document.uri, deleteRange.start, newSource);
        }
    }

    protected compareSymbols(symbolA: AZSymbolInformation, symbolB: AZSymbolInformation): number {
        let subtypeA = this.preProcessTypeName(symbolA.elementsubtype);
        let subtypeB = this.preProcessTypeName(symbolB.elementsubtype);
        
        let result: number = this.getTypePriority(subtypeA) - this.getTypePriority(subtypeB);        
        if (result != 0)
            return result;
        
        if ((subtypeA) && (subtypeB)) {
            result = subtypeA.localeCompare(subtypeB, undefined, { numeric: true, sensitivity: 'base' });
            if (result != 0)
                return result;
        }

        subtypeA = this.preProcessTypeName(symbolA.subtype);
        subtypeB = this.preProcessTypeName(symbolB.subtype);
        if ((subtypeA) && (subtypeB)) {
            result = subtypeA.localeCompare(subtypeB, undefined, { numeric: true, sensitivity: 'base' });
            if (result != 0)
                return result;
        }

        return symbolA.name.localeCompare(symbolB.name, undefined, { numeric: true, sensitivity: 'base' });
    }

    protected getTypePriority(typeName: string | undefined) {
        if (typeName) {
            typeName = typeName.toLowerCase();
            for (let i=0; i<this._typeByPriority.length; i++)
                if (typeName.startsWith(this._typeByPriority[i]))
                    return i;
        }
        return this._typeByPriority.length;
    }

    protected preProcessTypeName(name: string | undefined) {
        if (!name)
            return name;
        return name.replace(/"/g, '');
    }

}