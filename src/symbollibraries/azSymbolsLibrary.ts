'use strict';

import * as vscode from 'vscode';
import { AZSymbolInformation } from "./azSymbolInformation";
import { AZSymbolKind } from './azSymbolKind';

export class AZSymbolsLibrary {
    displayName : string;
    name : string;
    rootSymbol : AZSymbolInformation | undefined;
    objectListRootSymbol : AZSymbolInformation | undefined;
    showObjectIds : Boolean;
    objectListEnabled : boolean;

    private _onSymbolsChanged: vscode.EventEmitter<AZSymbolsLibrary> = new vscode.EventEmitter<AZSymbolsLibrary>();
	readonly onSymbolsChanged: vscode.Event<AZSymbolsLibrary> = this._onSymbolsChanged.event;

    constructor() {
        this.showObjectIds = false;
        this.displayName = '';
        this.name = '';
        this.rootSymbol = undefined;
        this.objectListRootSymbol = undefined;
        this.objectListEnabled = true;
    }

    async loadAsync(forceReload : boolean) : Promise<boolean> {
        let loaded : boolean = await this.loadInternalAsync(forceReload);
        if (loaded)
            this.updateObjectList();

        if ((loaded) && (this._onSymbolsChanged))
            this._onSymbolsChanged.fire(this);
            
        return loaded;
    }

    public setRootSymbol(symbol : AZSymbolInformation) {
        this.rootSymbol = symbol;
        if (this._onSymbolsChanged)
            this._onSymbolsChanged.fire(this);
    }

    protected async loadInternalAsync(forceReload : boolean) : Promise<boolean> {
        return false;
    }

    public clear() {
        this.displayName ='';
        this.name = '';
        this.clearSymbols();
    }

    protected clearSymbols() {
        this.rootSymbol = undefined;
        this.objectListRootSymbol = undefined;
    }

    public updateObjectList() {
        if (this.rootSymbol) {
            this.rootSymbol.updateTree(true);
            if (this.objectListEnabled)
                this.objectListRootSymbol = this.rootSymbol.toObjectTree();
        }
    }

    public search(text : string) : AZSymbolInformation | undefined {
        if ((text) && (this.objectListRootSymbol) && (text != ''))
            return this.objectListRootSymbol.search(text);
        return undefined;
    }

    public getObjectSymbolByPath(path : number[] | undefined) : AZSymbolInformation | undefined {
        let object : AZSymbolInformation | undefined = this.getSymbolByPath(path);
        if ((object) && (object.isALObject()))
            return object;
        return undefined;
    }

    public getObjectSymbolListByPath(pathList: number[][]) : AZSymbolInformation[] {
        let symbolList : AZSymbolInformation[] = [];
        for (let i=0; i<pathList.length; i++) {
            let symbol = this.getObjectSymbolByPath(pathList[i]);
            if (symbol)
                symbolList.push(symbol);
        }
        return symbolList;
    }

    public getSymbolListByPath(pathList: number[][], kind : AZSymbolKind) : AZSymbolInformation[] {
        let symbolList : AZSymbolInformation[] = [];
        for (let i=0; i<pathList.length; i++) {
            let symbol = this.getSymbolByPath(pathList[i]);
            if ((symbol) && (symbol.kind == kind))
                symbolList.push(symbol);
        }
        return symbolList;
    }

    public getSymbolByPath(path : number[] | undefined) : AZSymbolInformation | undefined {
        if ((this.rootSymbol) && (path) && (path.length > 0)) {
            let symbol : AZSymbolInformation = this.rootSymbol;
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

} 