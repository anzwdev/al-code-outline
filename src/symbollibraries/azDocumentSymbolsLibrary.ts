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
import { TextEditorHelper } from '../tools/textEditorHelper';

export class AZDocumentSymbolsLibrary extends AZSymbolsLibrary {
    protected _docUri : vscode.Uri | undefined;
    protected _reloadRequired : boolean;
    protected _context : DevToolsExtensionContext;
    private _document: vscode.TextDocument | undefined;

    constructor(context : DevToolsExtensionContext, docUri : vscode.Uri | undefined, document?: vscode.TextDocument) {
        super();
        this._twoWayTree = true;
        
        this._context = context;
        this._docUri = docUri;
        this._document = document;
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

    getDocUri() : vscode.Uri | undefined {
        return this._docUri;
    }

    protected async GetDocumentAsync(): Promise<vscode.TextDocument | undefined> {
        if (this._document)
            return this._document;
        
        let editor : vscode.TextEditor | undefined = TextEditorHelper.findDocumentEditor(this._docUri);
        if (editor)
            return editor.document;
        if (this._docUri)
            return await vscode.workspace.openTextDocument(this._docUri);
        return undefined;
    }

    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        if ((!forceReload) && (!this._reloadRequired))
            return false;

        let newRootSymbol : AZSymbolInformation | undefined = undefined;

        //get document symbols
        let document = await this.GetDocumentAsync();

        if (document) {
            if (document.uri)
                this._docUri = document.uri;
            let symbolsLoad : Thenable<vscode.SymbolInformation[] | vscode.DocumentSymbol[] | undefined> = 
                vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', this._docUri);

            if (document.languageId == "al") {
                
                //al language - use our special language server to parse source code
                let source : string = document.getText();
        
                let documentPath : string = "";
                if ((this._docUri) && (this._docUri.fsPath))
                    documentPath = this._docUri.fsPath;

                let request : ToolsDocumentSymbolsRequest = new ToolsDocumentSymbolsRequest(source, documentPath, true);
                let response : ToolsDocumentSymbolsResponse | undefined = await this._context.toolsLangServerClient.getALDocumentSymbols(request);
                if ((response) && (response.root)) {
                    newRootSymbol = AZSymbolInformation.fromAny(response.root);                    
                    //merge symbols with symbols returned from Microsoft AL Language Extension
                    let symbols = undefined;
                    try {
                        symbols = await symbolsLoad;
                    }
                    catch (e) {
                        symbols = undefined;
                    }

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
                let symbols = undefined;
                try {
                    symbols = await symbolsLoad;
                }
                catch (e) {
                    symbols = undefined;
                }
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
    
    protected mergeFieldSymbolNames(azSymbol : AZSymbolInformation | undefined, vsFieldSymbols : vscode.DocumentSymbol[]) {
        if (!azSymbol)
            return;
        if ((azSymbol.kind == AZSymbolKind.PageField) || 
            (azSymbol.kind == AZSymbolKind.QueryColumn) || 
            (azSymbol.kind == AZSymbolKind.QueryFilter)) {
            if (azSymbol.range) {
                let vsSymbol = this.findVsCodeSymbolByRange(vsFieldSymbols, azSymbol.range);
                if (vsSymbol)
                    azSymbol.fullName = vsSymbol.name;
            }
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


} 