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
        this.objectListEnabled = false;
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

        this.clearSymbols();

        //get document symbols
        let editor : vscode.TextEditor | undefined = this.findTextEditor(this._docUri);
        if (editor) {
            let symbolsLoad : Thenable<vscode.SymbolInformation[] | vscode.DocumentSymbol[] | undefined> =
                vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', this._docUri);

            if (editor.document.languageId == "al") {
                
                //al language - use our special language server to parse source code
                let source : string;
                if (editor)
                    source = editor.document.getText();
                else
                    source = "";
        
                let request : ToolsDocumentSymbolsRequest = new ToolsDocumentSymbolsRequest(source, this._docUri.fsPath);
                let response : ToolsDocumentSymbolsResponse | undefined = await this._context.toolsLangServerClient.GetALDocumentSymbols(request);
                if ((response) && (response.root)) {
                    this.rootSymbol = AZSymbolInformation.fromAny(response.root);                    
                    //merge symbols with symbols returned from Microsoft AL Language Extension
                    let symbols = await symbolsLoad;
                    if (symbols) {
                        let fieldSymbols = this.collectVsCodeFieldSymbols(symbols);
                        if ((fieldSymbols) && (fieldSymbols.length > 0))
                            this.mergeFieldSymbolNames(this.rootSymbol, fieldSymbols);
                    }
                } else
                    this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);
            } else {
                //use standard visual studio code symbols functionality to load symbols
                this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);                
                let symbols = await symbolsLoad;
                    //vscode.commands.executeCommand<vscode.SymbolInformation[] | vscode.DocumentSymbol[]>('vscode.executeDocumentSymbolProvider', this._docUri);
                if ((symbols) && (symbols.length > 0)) {
                    if (this.isDocumentSymbolsList(symbols))
                        this.loadDocumentSymbols(this.rootSymbol, symbols as vscode.DocumentSymbol[]);
                    else
                        this.loadSymbolsInformation(this.rootSymbol, symbols as vscode.SymbolInformation[]);
                }
            }

        }
        return true;
    }

    //#region update field names from our language server with values returned by Microsoft AL Language Extension 
    
    protected mergeFieldSymbolNames(azSymbol : AZSymbolInformation, vsFieldSymbols : vscode.DocumentSymbol[]) {
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

    /*
    protected loadFromSymbolInformationList(parent: ALSymbolInfo, symbols : vscode.SymbolInformation[]) {
        if (symbols) {
            let stack : ALSymbolInfo[] = [];
            //add nodes one by one
            stack.push(parent);

            for (let i = 0; i<symbols.length;i++) {
                let currNode : ALSymbolInfo = this.createFromVSCodeSymbol(symbols[i].kind, symbols[i].name);
                //append child node
                while ((stack.length > 1) && (stack[stack.length - 1].name != symbols[i].containerName)) {
                    stack.pop();
                }
                stack[stack.length - 1].addChildItem(currNode);
                stack.push(currNode);
            }
        }
    }

    protected loadFromDocumentSymbolList(parent: ALSymbolInfo, symbols : vscode.DocumentSymbol[]) {
        if (symbols) {
            for (let i=0; i<symbols.length; i++) {            
                let alSymbol = this.createFromVSCodeSymbol(symbols[i].kind, symbols[i].name);
                this.loadFromDocumentSymbolList(alSymbol, symbols[i].children);
                parent.addChildItem(alSymbol);
            }
        }
    }

    protected getSymbolPrefix(kind : ALSymbolKind) : string {
        switch (kind) {
            case ALSymbolKind.Field:
                return "Field ";
            case ALSymbolKind.PageObject:
                return "Page ";
            case ALSymbolKind.TableObject:
                return "Table ";
        }        
        return "";
    }

    protected createFromVSCodeSymbol(vscodeSymbolKind : vscode.SymbolKind, name : string) : ALSymbolInfo {
        let symbol : ALSymbolInfo = ALSymbolInfo.create(this.vsSymbolKindToAlSymbolKind(vscodeSymbolKind, name), ALSyntaxHelper.fromNameText(name));
        let prefix : string = this.getSymbolPrefix(symbol.kind);

        //update symbol name
        if (symbol.kind == ALSymbolKind.Field) {
            let pos : number = name.lastIndexOf(':');
            if (pos >= 0)
                name = name.substr(0, pos).trim();
        }
        //symbol name prefix
        if ((prefix != "") && (name.startsWith(prefix)))
            name = name.substr(prefix.length).trim();
        //update name
        symbol.name = ALSyntaxHelper.fromNameText(name);

        return symbol;
    }

    protected vsSymbolKindToAlSymbolKind(vscodeSymbolKind : vscode.SymbolKind, name : string) : ALSymbolKind {
        switch (vscodeSymbolKind) {
            case vscode.SymbolKind.File:
                return ALSymbolKind.Document;
            case vscode.SymbolKind.Module:
                return ALSymbolKind.SymbolGroup;
            case vscode.SymbolKind.Namespace:
                return ALSymbolKind.SymbolGroup;
            case vscode.SymbolKind.Package:
                return ALSymbolKind.SymbolGroup;
            case vscode.SymbolKind.Class:
                return this.classNameToAlSymbolKind(name);
            case vscode.SymbolKind.Method:
                return ALSymbolKind.MethodDeclaration;
            case vscode.SymbolKind.Property:
                return ALSymbolKind.Property;
            case vscode.SymbolKind.Field:
                return ALSymbolKind.Field;
            case vscode.SymbolKind.Constructor:
                return ALSymbolKind.MethodDeclaration;
            case vscode.SymbolKind.Enum:
                return ALSymbolKind.EnumType;
            case vscode.SymbolKind.Interface:
                return ALSymbolKind.UndefinedObject;
            case vscode.SymbolKind.Function:
                return ALSymbolKind.MethodDeclaration;
            case vscode.SymbolKind.Variable:
                return ALSymbolKind.Variable;
            case vscode.SymbolKind.Constant:
                return ALSymbolKind.Constant;
            case vscode.SymbolKind.String:
                return ALSymbolKind.Constant;
            case vscode.SymbolKind.Number:
                return ALSymbolKind.Constant;
            case vscode.SymbolKind.Boolean:
                return ALSymbolKind.Constant;
            case vscode.SymbolKind.Array:
                return ALSymbolKind.Variable;
            case vscode.SymbolKind.Object:
                return this.objectNameToAlSymbolKind(name);
            case vscode.SymbolKind.Key:
                return ALSymbolKind.Key;
            case vscode.SymbolKind.Null:
                return ALSymbolKind.Constant;
            case vscode.SymbolKind.EnumMember:
                return ALSymbolKind.EnumValue;
            case vscode.SymbolKind.Struct:
                return ALSymbolKind.UndefinedObject;
            case vscode.SymbolKind.Event:
                return ALSymbolKind.EventPublisher;
            case vscode.SymbolKind.Operator:
                return ALSymbolKind.Undefined;
            case vscode.SymbolKind.TypeParameter:
                return ALSymbolKind.Parameter;
        }

        return ALSymbolKind.Undefined;
    }

    protected classNameToAlSymbolKind(symbolName : string) : ALSymbolKind {
        symbolName = symbolName.toLowerCase();
        if (symbolName.startsWith("table"))
            return ALSymbolKind.Table;
        if (symbolName.startsWith("codeunit"))
            return ALSymbolKind.Codeunit;
        if (symbolName.startsWith("page"))
            return ALSymbolKind.Page;
        if (symbolName.startsWith("report"))
            return ALSymbolKind.Report;
        if (symbolName.startsWith("query"))
            return ALSymbolKind.Query;
        if (symbolName.startsWith("xmlport"))
            return ALSymbolKind.XmlPort;
        if (symbolName.startsWith("tableextension"))
            return ALSymbolKind.TableExtension;
        if (symbolName.startsWith("pageextension"))
            return ALSymbolKind.PageExtension;
        return ALSymbolKind.UndefinedObject;
    }

    protected objectNameToAlSymbolKind(symbolName : string) : ALSymbolKind {
        symbolName = symbolName.toLowerCase();
        if (symbolName.startsWith("tableextension"))
            return ALSymbolKind.TableExtension;
        if (symbolName.startsWith("pageextension"))
            return ALSymbolKind.PageExtension;
        if (symbolName.startsWith("key "))
            return ALSymbolKind.Key;
        if (symbolName.startsWith("fieldgroup"))
            return ALSymbolKind.FieldGroup;
        if (symbolName.startsWith("action "))
            return ALSymbolKind.Action;
        
        //page fields
        if (symbolName.startsWith("field "))
            return ALSymbolKind.Field;
        if (symbolName.startsWith("usercontrol "))
            return ALSymbolKind.Field;
        if (symbolName.startsWith("label "))
            return ALSymbolKind.Constant;

        //page field group controls
        if (symbolName.startsWith("group "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("cuegroup "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("fixed "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("grid "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("area "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("repeater "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("part "))
            return ALSymbolKind.Group;
        if (symbolName.startsWith("systempart "))
            return ALSymbolKind.Group;

        //enum
        if (symbolName.startsWith("enum value "))
            return ALSymbolKind.EnumValue;
        if (symbolName.startsWith("enum "))
            return ALSymbolKind.Enum;
        if (symbolName.startsWith("enumextension "))
            return ALSymbolKind.EnumExtension;

        return ALSymbolKind.Undefined;       
    }
    */

} 