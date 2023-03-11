'use strict';

import * as vscode from 'vscode';
import { ALSymbolSourceLocation } from './alSymbolSourceLocation';
import { AZSymbolInformation } from "./azSymbolInformation";
import { AZSymbolKind } from './azSymbolKind';

export class AZSymbolsLibrary {
    displayName : string;
    name : string;
    rootSymbol : AZSymbolInformation | undefined;
    showObjectIds : boolean;
    protected _twoWayTree: boolean;
    protected _sourceId: string | undefined;

    private _onSymbolsChanged: vscode.EventEmitter<AZSymbolsLibrary> = new vscode.EventEmitter<AZSymbolsLibrary>();
	readonly onSymbolsChanged: vscode.Event<AZSymbolsLibrary> = this._onSymbolsChanged.event;

    constructor() {
        this.showObjectIds = false;
        this.displayName = '';
        this.name = '';
        this.rootSymbol = undefined;
        this._twoWayTree = false;
        this._sourceId = undefined;
    }

    async loadAsync(forceReload : boolean) : Promise<boolean> {
        let loaded : boolean = await this.loadInternalAsync(forceReload);
        if (loaded)
            this.updateObjectList();

        if ((loaded) && (this._onSymbolsChanged))
            this._onSymbolsChanged.fire(this);
            
        return loaded;
    }

    loadFromAny(source: any) {
        if (source)
            this.rootSymbol = AZSymbolInformation.fromAny(source);
        else
            this.rootSymbol = AZSymbolInformation.create(AZSymbolKind.Document, this.displayName);
        this.updateObjectList();
        if (this._onSymbolsChanged)
            this._onSymbolsChanged.fire(this);
    }

    public setRootSymbol(symbol : AZSymbolInformation | undefined, sourceId: string | undefined) {
        this.rootSymbol = symbol;
        this._sourceId = sourceId;
        if (this._onSymbolsChanged)
            this._onSymbolsChanged.fire(this);
    }

    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        return false;
    }

    public async unloadAsync() {        
    }

    public clear() {
        this.displayName ='';
        this.name = '';
        this.clearSymbols();
    }

    protected clearSymbols() {
        this.rootSymbol = undefined;
    }

    public updateObjectList() {
        if (this.rootSymbol) {
            this.rootSymbol.updateTree(true, this._twoWayTree);
        }
    }

    public async getSymbolsListByPathAsync(pathList: number[][], kind : AZSymbolKind) : Promise<AZSymbolInformation[]> {
        let symbolList : AZSymbolInformation[] = [];
        for (let i=0; i<pathList.length; i++) {
            let symbol = this.getSymbolByPath(pathList[i]);
            if ((symbol) && ((kind == AZSymbolKind.Undefined) || (symbol.kind == kind) || ((kind == AZSymbolKind.AnyALObject) && (symbol.isALObject()))))
                symbolList.push(symbol);
        }
        return symbolList;
    }

    protected getSymbolByPath(path : number[] | undefined) : AZSymbolInformation | undefined {
        if (this.rootSymbol)
            return this.getSymbolByPathWithRoot(this.rootSymbol, path);
        return undefined;
    }

    protected getSymbolByPathWithRoot(root: AZSymbolInformation, path : number[] | undefined) : AZSymbolInformation | undefined {
        if ((this.rootSymbol) && (root) && (path) && (path.length > 0)) {
            let symbol = this.rootSymbol;
            for (let i=path.length-1; i>=0; i--) {
                if ((!symbol.childSymbols) || (path[i] >= symbol.childSymbols.length))
                    return undefined;
                if (path[i] == -1)
                    symbol = this.rootSymbol;
                else
                    symbol = symbol.childSymbols[path[i]];
            }
            return symbol;
        }
        return undefined;
    }

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
        return this.findSymbolInRangeInt(this.rootSymbol, range, undefined);
    }

    findSymbolsInsideRange(range: vscode.Range, kind: AZSymbolKind, list: AZSymbolInformation[]) {
        if (this.rootSymbol)
            this.findSymbolsInsideRangeInt(this.rootSymbol, range, kind, list);    
    }

    findALObjectsInsideRange(range: vscode.Range, list: AZSymbolInformation[]) {
        if (this.rootSymbol)
            this.findALObjectsInsideRangeInt(this.rootSymbol, range, list);    
    }

    findSymbolPathInRange(range: vscode.Range) : number[] | undefined {
        if ((range) && (this.rootSymbol)) {
            let symbolsPath: number[] = [];
            this.findSymbolInRangeInt(this.rootSymbol, range, symbolsPath);
            if (symbolsPath.length > 0)
                return symbolsPath;
        }
        return undefined;
    }

    findSymbolPathInSelectionRange(range: vscode.Range) : number[] | undefined {
        if ((range) && (this.rootSymbol)) {
            let symbolsPath: number[] = [];
            this.findSymbolInSelectionRangeInt(this.rootSymbol, range, symbolsPath);
            if (symbolsPath.length > 0)
                return symbolsPath;
        }
        return undefined;
    }

    protected findSymbolsInsideRangeInt(symbol: AZSymbolInformation, range: vscode.Range, kind: AZSymbolKind, list: AZSymbolInformation[]) {
        if ((symbol.kind == kind) && (symbol.range) && (symbol.range.insideVsRange(range)))
            list.push(symbol);

        if (symbol.childSymbols)
            for (let i=0; i<symbol.childSymbols.length; i++)
                this.findSymbolsInsideRangeInt(symbol.childSymbols[i], range, kind, list);
    }

    protected findALObjectsInsideRangeInt(symbol: AZSymbolInformation, range: vscode.Range, list: AZSymbolInformation[]) {
        if (symbol.isALObject()) {
            if ((symbol.range) && (symbol.range.insideVsRange(range)))
                list.push(symbol);
        } else {
            if (symbol.childSymbols)
                for (let i=0; i<symbol.childSymbols.length; i++)
                    this.findALObjectsInsideRangeInt(symbol.childSymbols[i], range, list);
        }       
    }

    protected findSymbolInRangeInt(symbol: AZSymbolInformation, range: vscode.Range, symbolsPath: number[] | undefined) : AZSymbolInformation | undefined {
        let found : AZSymbolInformation | undefined = undefined;            
        if ((symbol.range) && (symbol.range.intersectVsRange(range)))
            found = symbol;

        if (symbol.childSymbols) {
            for (let i=0; i<symbol.childSymbols.length; i++) {
                let foundChild = this.findSymbolInRangeInt(symbol.childSymbols[i], range, symbolsPath);
                if (foundChild) {
                    if (symbolsPath)
                        symbolsPath.push(i);
                    return foundChild;
                }
            }
        }

        return found;
    }

    protected findSymbolInSelectionRangeInt(symbol: AZSymbolInformation, range: vscode.Range, symbolsPath: number[] | undefined) : AZSymbolInformation | undefined {
        let found : AZSymbolInformation | undefined = undefined;
        
        if (symbol.selectionRange) {
            if (symbol.selectionRange.intersectVsRange(range))
                found = symbol;
        } else if ((symbol.range) && (symbol.range.intersectVsRange(range)))
            found = symbol;

        if (symbol.childSymbols) {
            for (let i=0; i<symbol.childSymbols.length; i++) {
                let foundChild = this.findSymbolInSelectionRangeInt(symbol.childSymbols[i], range, symbolsPath);
                if (foundChild) {
                    if (symbolsPath)
                        symbolsPath.push(i);
                    return foundChild;
                }
            }
        }

        return found;
    }

    //#region

    public async getSymbolLocationByPath(symbolPath: number[]) : Promise<ALSymbolSourceLocation | undefined> {
        return undefined;
    }

    public getUri(): vscode.Uri | undefined {
        return undefined;
    }

    public getSourceId(): string {
        let uri = this.getUri();
        if (uri)
            return uri.toString();
        if (this._sourceId)
            return this._sourceId;
        return 'undefined';
    }

} 