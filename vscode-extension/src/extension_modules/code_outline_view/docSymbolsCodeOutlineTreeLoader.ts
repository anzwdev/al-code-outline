import * as vscode from 'vscode';
import { CodeOutlineTreeLoader } from "./codeOutlineTreeLoader";
import { DevToolsExtensionContext } from '../../devToolsExtensionContext';
import { CodeOutlineTreeDocumentState } from './codeOutlineTreeDocumentState';
import { CodeOutlineTreeItem } from './codeOutlineTreeItem';
import { SyntaxTreeSymbolsIcons } from '../syntax_tree_symbols/syntaxTreeSymbolsIcons';
import { LSSyntaxNodeKind } from '../../langserver/common_types/lsSyntaxNodeKind';
import { LSSyntaxNodeAccessModifier } from '../../langserver/common_types/lsSyntaxNodeAccessModifier';

export class DocSymbolsCodeOutlineTreeLoader implements CodeOutlineTreeLoader {

    public getTreeId(): string | undefined {
        return vscode.window.activeTextEditor?.document.uri.toString();
    }

    public  async loadTree(context: DevToolsExtensionContext, state: CodeOutlineTreeDocumentState): Promise<CodeOutlineTreeItem | undefined> {
        let document = vscode.window.activeTextEditor?.document;
        if (!document) {
            return undefined;
        }

        let lastId = 1;
        let rootItem = new CodeOutlineTreeItem(lastId.toString(), "Symbols", LSSyntaxNodeKind.CompilationUnit, undefined, undefined, undefined, undefined, state);

        let symbols = await vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', document.uri);
        if ((!symbols) || (symbols.length === 0)) {
            return rootItem;
        }

        let documentSymbols = this.isDocumentSymbolsList(symbols);
        rootItem.childNodes = [];
        if (documentSymbols) {
            rootItem.updateChildNodes(this.createDocumentTreeItemsList(context.vscodeExtensionContext, symbols as vscode.DocumentSymbol[], rootItem, state, lastId).treeItems, state);
        } else {
            rootItem.updateChildNodes(this.createSymbolTreeItemsList(context.vscodeExtensionContext, symbols as vscode.SymbolInformation[], rootItem, state, lastId).treeItems, state);
        }

        return rootItem;
    }

    private createDocumentTreeItemsList(context: vscode.ExtensionContext, symbols: vscode.DocumentSymbol[], parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState, lastId: number) : { lastId: number, treeItems: CodeOutlineTreeItem[] } {
        let treeItems: CodeOutlineTreeItem[] = [];

        for (let i=0; i < symbols.length; i++) {
            let val = this.createDocumentTreeItem(context, symbols[i], parent, state, lastId);
            lastId = val.lastId;
            treeItems.push(val.treeItem);
        }

        return { lastId: lastId, treeItems: treeItems };
    }

    private createDocumentTreeItem(context: vscode.ExtensionContext, symbol: vscode.DocumentSymbol, parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState, lastId: number) : { lastId: number, treeItem: CodeOutlineTreeItem } {
        lastId++;

        let kind = this.convertSymbolKind(symbol.kind);
        let iconPath = SyntaxTreeSymbolsIcons.getIconPath(context, kind, LSSyntaxNodeAccessModifier.Public, "");
        let treeItem = new CodeOutlineTreeItem(lastId.toString(), symbol.name, kind, symbol.range, symbol.selectionRange, iconPath, parent, state);

        if ((symbol.children) && (symbol.children.length > 0)) {
            let val = this.createDocumentTreeItemsList(context, symbol.children, treeItem, state, lastId);
            lastId = val.lastId;
            treeItem.updateChildNodes(val.treeItems, state);
        }

        return { lastId, treeItem };
    }

    private createSymbolTreeItemsList(context: vscode.ExtensionContext, symbols: vscode.SymbolInformation[], parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState, lastId: number) : { lastId: number, treeItems: CodeOutlineTreeItem[] } {
        let treeItems: CodeOutlineTreeItem[] = [];

        for (let i=0; i < symbols.length; i++) {
            let val = this.createSymbolTreeItem(context, symbols[i], parent, state, lastId);
            lastId = val.lastId;
            treeItems.push(val.treeItem);
        }

        return { lastId: lastId, treeItems: treeItems };
    }

    private createSymbolTreeItem(context: vscode.ExtensionContext, symbol: vscode.SymbolInformation, parent: CodeOutlineTreeItem | undefined, state: CodeOutlineTreeDocumentState, lastId: number) : { lastId: number, treeItem: CodeOutlineTreeItem } {
        lastId++;

        let kind = this.convertSymbolKind(symbol.kind);
        let iconPath = SyntaxTreeSymbolsIcons.getIconPath(context, kind, LSSyntaxNodeAccessModifier.Public, "");
        let treeItem = new CodeOutlineTreeItem(lastId.toString(), symbol.name, kind, symbol.location?.range, symbol.location?.range, iconPath, parent, state);

        return { lastId, treeItem };
    }

    protected isDocumentSymbolsList(symbols : any[]) : boolean {
        for (let i=0; i<symbols.length; i++) {
            if (symbols[i].children) {
                return true;
            }
        }
        return false;
    }

    private convertSymbolKind(kind: vscode.SymbolKind): LSSyntaxNodeKind {
        switch (kind) {
            case vscode.SymbolKind.File: return LSSyntaxNodeKind.Document;
            case vscode.SymbolKind.Module: return LSSyntaxNodeKind.CodeunitObject;
            case vscode.SymbolKind.Namespace: return LSSyntaxNodeKind.Namespace;
            case vscode.SymbolKind.Package: return LSSyntaxNodeKind.Package;
            case vscode.SymbolKind.Class: return LSSyntaxNodeKind.Class;
            case vscode.SymbolKind.Method: return LSSyntaxNodeKind.MethodDeclaration;
            case vscode.SymbolKind.Property: return LSSyntaxNodeKind.Property;
            case vscode.SymbolKind.Field: return LSSyntaxNodeKind.Field;
            case vscode.SymbolKind.Constructor: return LSSyntaxNodeKind.Constructor;
            case vscode.SymbolKind.Enum: return LSSyntaxNodeKind.EnumType;
            case vscode.SymbolKind.Interface: return LSSyntaxNodeKind.Interface;
            case vscode.SymbolKind.Function: return LSSyntaxNodeKind.LocalMethodDeclaration;            
            case vscode.SymbolKind.Variable: return LSSyntaxNodeKind.VariableDeclaration;
            case vscode.SymbolKind.Constant: return LSSyntaxNodeKind.Constant;
            case vscode.SymbolKind.String: return LSSyntaxNodeKind.String;
            case vscode.SymbolKind.Number: return LSSyntaxNodeKind.Number;
            case vscode.SymbolKind.Boolean: return LSSyntaxNodeKind.Boolean;
            case vscode.SymbolKind.Array: return LSSyntaxNodeKind.Array;
            case vscode.SymbolKind.Object: return LSSyntaxNodeKind.Object;
            case vscode.SymbolKind.Key: return LSSyntaxNodeKind.Key;
            case vscode.SymbolKind.Null: return LSSyntaxNodeKind.Null;
            case vscode.SymbolKind.EnumMember: return LSSyntaxNodeKind.EnumValue;
            case vscode.SymbolKind.Struct: return LSSyntaxNodeKind.Struct;
            case vscode.SymbolKind.Event: return LSSyntaxNodeKind.EventDeclaration;
            case vscode.SymbolKind.Operator: return LSSyntaxNodeKind.Operator;
            case vscode.SymbolKind.TypeParameter: return LSSyntaxNodeKind.Parameter;
        }
        return LSSyntaxNodeKind.Undefined;
    }

}