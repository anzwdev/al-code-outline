import { ALAppSymbolsLibrary } from "./alAppSymbolsLibrary";
import { DevToolsExtensionContext } from "../devToolsExtensionContext";

'use strict';

export class ALAppSymbolsLibrariesCache {
    protected _cache : ALAppSymbolsLibrary[];
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
        let library : ALAppSymbolsLibrary = new ALAppSymbolsLibrary(this._context, filePath);
        this._cache.push(library);
        return library;
    }

} 