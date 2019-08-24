'use strict';

import * as vscode from 'vscode';
import * as path from 'path';
import { AZSymbolsLibrary } from './azSymbolsLibrary';
import { AZSymbolKind } from './azSymbolKind';
import { DevToolsExtensionContext } from '../devToolsExtensionContext';
import { ToolsDocumentSymbolsRequest } from '../langserver/toolsDocumentSymbolsRequest';
import { ToolsDocumentSymbolsResponse } from '../langserver/toolsDocumentSymbolsResponse';
import { TextRange } from './textRange';
import { AZSymbolInformation } from './azSymbolInformation';

export class AZDocumentSymbolsLibrary extends AZSymbolsLibrary {
    protected _docUri : vscode.Uri | undefined;
    protected _reloadRequired : boolean;
    protected _context : DevToolsExtensionContext;

    constructor(context : DevToolsExtensionContext, docUri : vscode.Uri | undefined) {
        super();
        this._twoWayTree = true;
        
        this._context = context;
        this._docUri = docUri;
        this._reloadRequired = true;
        if (this._docUri) {
            this.name = this._docUri.fsPath;
            this.displayName = path.basename(this._docUri.fsPath);
        }
    }

    setDocUri(newUri : vscode.Uri | undefined) {
        this._docUri = newUri;
        this._reloadRequired = true;
    }

    protected findTextEditor(uri : vscode.Uri | undefined) : vscode.TextEditor | undefined {
        if ((uri) && (vscode.window.visibleTextEditors)) {
            for (let i=0; i<vscode.window.visibleTextEditors.length; i++) {
                if (vscode.window.visibleTextEditors[i].document.uri.fsPath == uri.fsPath)
                    return vscode.window.visibleTextEditors[i];
            }
        }
        return undefined;
    }

    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        if ((!forceReload) && (!this._reloadRequired))
            return false;

        let newRootSymbol : AZSymbolInformation | undefined = undefined;

        //get document symbols
        let editor : vscode.TextEditor | undefined = this.findTextEditor(this._docUri);
        if (editor) {
            if ((editor.document) && (editor.document.uri))
                this._docUri = editor.document.uri;
            let symbolsLoad : Thenable<vscode.SymbolInformation[] | vscode.DocumentSymbol[] | undefined> =
                vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', this._docUri);

            if (editor.document.languageId == "al") {
                
                //al language - use our special language server to parse source code
                let source : string;
                if (editor)
                    source = editor.document.getText();
                else
                    source = "";
        
                let documentPath : string = "";
                if ((this._docUri) && (this._docUri.fsPath))
                    documentPath = this._docUri.fsPath;

                let request : ToolsDocumentSymbolsRequest = new ToolsDocumentSymbolsRequest(source, documentPath);
                let response : ToolsDocumentSymbolsResponse | undefined = await this._context.toolsLangServerClient.getALDocumentSymbols(request);
                if ((response) && (response.root)) {
                    newRootSymbol = AZSymbolInformation.fromAny(response.root);                    
                    //merge symbols with symbols returned from Microsoft AL Language Extension
                    let symbols = await symbolsLoad;
                    if (symbols) {
                        let fieldSymbols = this.collectVsCodeFieldSymbols(symbols);
                        if ((fieldSymbols) && (fieldSymbols.length > 0))
                            this.mergeFieldSymbolNames(this.rootSymbol, fieldSymbols);
                    }
                } else
                    newRootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);
            } else {
                //use standard visual studio code symbols functionality to load symbols
                newRootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);                
                let symbols = await symbolsLoad;
                    //vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', this._docUri);
                if ((symbols) && (symbols.length > 0)) {
                    if (this.isDocumentSymbolsList(symbols))
                        this.loadDocumentSymbols(newRootSymbol, symbols as vscode.DocumentSymbol[]);
                    else
                        this.loadSymbolsInformation(newRootSymbol, symbols as vscode.SymbolInformation[]);
                }
            }

            this.rootSymbol = newRootSymbol;
        }
        return true;
    }

    //#region update field names from our language server with values returned by Microsoft AL Language Extension 
    
    protected mergeFieldSymbolNames(azSymbol : AZSymbolInformation, vsFieldSymbols : vscode.DocumentSymbol[]) {
        if (!azSymbol)
            return;
        if ((azSymbol.kind == AZSymbolKind.PageField) || 
            (azSymbol.kind == AZSymbolKind.QueryColumn) || 
            (azSymbol.kind == AZSymbolKind.QueryFilter)) {
            let vsSymbol = this.findVsCodeSymbolByRange(vsFieldSymbols, azSymbol.range);
            if (vsSymbol)
                azSymbol.fullName = vsSymbol.name;
        } else if (azSymbol.childSymbols) {
            for (let i=0; i<azSymbol.childSymbols.length; i++) {
                this.mergeFieldSymbolNames(azSymbol.childSymbols[i], vsFieldSymbols);
            }
        }
    }

    protected findVsCodeSymbolByRange(symbols: vscode.DocumentSymbol[], range : TextRange) : vscode.DocumentSymbol | undefined {
        for (let i=0; i<symbols.length; i++) {
            if (range.intersectVsRange(symbols[i].range))
                return symbols[i];
        }
        return undefined;
    }

    protected collectVsCodeFieldSymbols(symbols : vscode.SymbolInformation[] | vscode.DocumentSymbol[] | undefined) : vscode.DocumentSymbol[] | undefined {
        if (!symbols)
            return undefined;
        let fieldList : vscode.DocumentSymbol[] = [];
        if (this.isDocumentSymbolsList(symbols))
            this.collectVSCodeFieldDocSymbols(symbols as vscode.DocumentSymbol[], fieldList);
        else
            this.collectVSCodeFieldSymbolInf(symbols as vscode.SymbolInformation[], fieldList);

        fieldList.sort(function(v1, v2){
            if (v1.range.start.line == v2.range.start.line) {
                if (v1.range.start.character == v2.range.start.character) {
                    if (v1.range.end.line == v2.range.end.line)
                        return v2.range.end.character - v1.range.end.character;
                    else
                        return v2.range.end.line - v1.range.end.line;
                } else 
                    return v2.range.start.character - v1.range.start.character;
            } else
                return v2.range.start.line - v1.range.start.line
            });

        return fieldList;
    }

    protected collectVSCodeFieldDocSymbols(symbols : vscode.DocumentSymbol[], collected: vscode.DocumentSymbol[]) {
        for (let i=0; i<symbols.length; i++) {
            //if (symbols[i].kind == vscode.SymbolKind.Field)
            //    collected.push(symbols[i]);
            if ((!symbols[i].children) || (symbols[i].children.length == 0))
                collected.push(symbols[i]);
            else
                this.collectVSCodeFieldDocSymbols(symbols[i].children, collected);           
        }
    }

    protected collectVSCodeFieldSymbolInf(symbols : vscode.SymbolInformation[], collected: vscode.DocumentSymbol[]) {
        for (let i=0; i<symbols.length; i++) {
            //if (symbols[i].kind == vscode.SymbolKind.Field)
                collected.push(new vscode.DocumentSymbol(symbols[i].name, '', symbols[i].kind,
                    symbols[i].location.range, symbols[i].location.range));
        }
    }

    //#endregion

    //#region standard vscode symbols processing

    protected isDocumentSymbolsList(symbols : any[]) : boolean {
        for (let i=0; i<symbols.length; i++) {
            if (symbols[i].children)
                return true;
        }
        return false;
    }

    protected loadDocumentSymbols(parent : AZSymbolInformation, vsSymbols : vscode.DocumentSymbol[]) {
        for (let i=0; i<vsSymbols.length; i++) {
            let vsSymbol = vsSymbols[i];
            let symbol : AZSymbolInformation = AZSymbolInformation.create(this.vsSymbolKindToazSymbolKind(vsSymbol.kind), vsSymbol.name);
            symbol.range = TextRange.fromAny(vsSymbol.range);
            symbol.selectionRange = TextRange.fromAny(vsSymbol.selectionRange);
            if ((vsSymbol.children) && (vsSymbol.children.length > 0))
                this.loadDocumentSymbols(symbol, vsSymbol.children);
            parent.addChildItem(symbol);
        }
    }

    protected loadSymbolsInformation(parent : AZSymbolInformation, vsSymbols : vscode.SymbolInformation[]) {
        for (let i=0; i<vsSymbols.length; i++) {
            let symbol : AZSymbolInformation = AZSymbolInformation.create(this.vsSymbolKindToazSymbolKind(vsSymbols[i].kind), vsSymbols[i].name);
            if ((vsSymbols[i].location) && (vsSymbols[i].location.range)) {
                symbol.range = TextRange.fromAny(vsSymbols[i].location.range);
                symbol.selectionRange = symbol.range;                
            }
            parent.addChildItem(symbol);
        }
    }

    protected vsSymbolKindToazSymbolKind(kind : vscode.SymbolKind) : AZSymbolKind {
        switch (kind) {
            case vscode.SymbolKind.File: return AZSymbolKind.Document;
            case vscode.SymbolKind.Module: return AZSymbolKind.CodeunitObject;
            case vscode.SymbolKind.Namespace: return AZSymbolKind.Namespace;
            case vscode.SymbolKind.Package: return AZSymbolKind.Package;
            case vscode.SymbolKind.Class: return AZSymbolKind.Class;
            case vscode.SymbolKind.Method: return AZSymbolKind.MethodDeclaration;
            case vscode.SymbolKind.Property: return AZSymbolKind.Property;
            case vscode.SymbolKind.Field: return AZSymbolKind.Field;
            case vscode.SymbolKind.Constructor: return AZSymbolKind.Constructor;
            case vscode.SymbolKind.Enum: return AZSymbolKind.EnumType;
            case vscode.SymbolKind.Interface: return AZSymbolKind.Interface;
            case vscode.SymbolKind.Function: return AZSymbolKind.LocalMethodDeclaration;
            case vscode.SymbolKind.Variable: return AZSymbolKind.VariableDeclaration;
            case vscode.SymbolKind.Constant: return AZSymbolKind.Constant;
            case vscode.SymbolKind.String: return AZSymbolKind.String;
            case vscode.SymbolKind.Number: return AZSymbolKind.Number;
            case vscode.SymbolKind.Boolean: return AZSymbolKind.Boolean;
            case vscode.SymbolKind.Array: return AZSymbolKind.Array;
            case vscode.SymbolKind.Object: return AZSymbolKind.Object;
            case vscode.SymbolKind.Key: return AZSymbolKind.Key;
            case vscode.SymbolKind.Null: return AZSymbolKind.Null;
            case vscode.SymbolKind.EnumMember: return AZSymbolKind.EnumValue;
            case vscode.SymbolKind.Struct: return AZSymbolKind.Struct;
            case vscode.SymbolKind.Event: return AZSymbolKind.EventDeclaration;
            case vscode.SymbolKind.Operator: return AZSymbolKind.Operator;
            case vscode.SymbolKind.TypeParameter: return AZSymbolKind.Parameter;
        }
        return AZSymbolKind.Undefined;
    }

    //#endregion

    //#region Symbols search

    findNextSymbol(line: number) : AZSymbolInformation | undefined {
        if ((this.rootSymbol) && (this.rootSymbol.childSymbols)) {
            for (let i=0; i<this.rootSymbol.childSymbols.length; i++) {
                let found = this.findNextSymbolInt(this.rootSymbol.childSymbols[i], line);
                if (found)
                    return found;
            }
        }
        return undefined;
    }

    protected findNextSymbolInt(symbol: AZSymbolInformation, line: number) : AZSymbolInformation | undefined {
        if ((symbol.range) && (symbol.range.start.line <= line) && (symbol.range.end.line >= line)) {

            if ((symbol.selectionRange) && (symbol.selectionRange.start.line >= line))
                return symbol;

            if (symbol.childSymbols) {
                for (let i=0; i<symbol.childSymbols.length; i++) {
                    let found = this.findNextSymbolInt(symbol.childSymbols[i], line);
                    if (found)
                        return found;                    
                }
            }
        }
        return undefined;
    }

    findSymbolInRange(range: vscode.Range) : AZSymbolInformation | undefined {
        if (!this.rootSymbol)
            return undefined;
        return this.findSymbolInRangeInt(this.rootSymbol, range);
    }

    protected findSymbolInRangeInt(symbol: AZSymbolInformation, range: vscode.Range) : AZSymbolInformation | undefined {
        let found : AZSymbolInformation | undefined = undefined;
        
        if (symbol.range.intersectVsRange(range)) {
            found = symbol;
        }

        if (symbol.childSymbols) {
            for (let i=0; i<symbol.childSymbols.length; i++) {
                let foundChild = this.findSymbolInRangeInt(symbol.childSymbols[i], range);
                if (foundChild)
                    return foundChild;
            }
        }

        return found;
    }

    //#region

} 