'use strict';

import { DevToolsExtensionContext } from "../../devToolsExtensionContext";
import { ALNativeAppSymbolsLibrary } from "./alNativeAppSymbolsLibrary";

export class ALNativeAppSymbolsLibrariesCache {
    protected _cache : ALNativeAppSymbolsLibrary[];
    protected _context : DevToolsExtensionContext;

    constructor(context : DevToolsExtensionContext) {
        this._context = context;
        this._cache = [];
    }

    clear() {
        this._cache = [];
    }

    getOrCreate(filePath : string) {
        //try to find library
        for (let i=0; i<this._cache.length; i++) {
            if (this._cache[i].filePath == filePath)
                return this._cache[i];
        }
        //create new library
        let library : ALNativeAppSymbolsLibrary = new ALNativeAppSymbolsLibrary(this._context, filePath);
        this._cache.push(library);
        return library;
    }

} 