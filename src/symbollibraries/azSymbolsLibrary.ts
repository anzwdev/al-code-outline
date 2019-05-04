'use strict';

import * as vscode from 'vscode';
import { AZSymbolInformation } from "./azSymbolInformation";
import { AZSymbolKind } from './azSymbolKind';

export class AZSymbolsLibrary {
    displayName : string;
    name : string;
    rootSymbol : AZSymbolInformation | undefined;
    showObjectIds : Boolean;

    private _onSymbolsChanged: vscode.EventEmitter<AZSymbolsLibrary> = new vscode.EventEmitter<AZSymbolsLibrary>();
	readonly onSymbolsChanged: vscode.Event<AZSymbolsLibrary> = this._onSymbolsChanged.event;

    constructor() {
        this.showObjectIds = false;
        this.displayName = '';
        this.name = '';
        this.rootSymbol = undefined;
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
            this.rootSymbol.updateTree(true);
        }
    }

    public async getSymbolsListByPathAsync(pathList: number[][], kind : AZSymbolKind) : Promise<AZSymbolInformation[]> {
        let symbolList : AZSymbolInformation[] = [];
        for (let i=0; i<pathList.length; i++) {
            let symbol = this.getSymbolByPath(pathList[i]);
            if ((symbol) && ((symbol.kind == kind) || ((kind == AZSymbolKind.AnyALObject) && (symbol.isALObject()))))
                symbolList.push(symbol);
        }
        return symbolList;
    }

    protected getSymbolByPath(path : number[] | undefined) : AZSymbolInformation | undefined {
        return this.getSymbolByPathWithRoot(this.rootSymbol, path);
    }

    protected getSymbolByPathWithRoot(root: AZSymbolInformation, path : number[] | undefined) : AZSymbolInformation | undefined {
        if ((root) && (path) && (path.length > 0)) {
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