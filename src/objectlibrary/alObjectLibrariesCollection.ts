import * as vscode from 'vscode';
import * as path from 'path';
import { ALObjectLibrary } from "./alObjectLibrary";
import { ALBasicLibrary } from "./alBasicLibrary";
import { ALSymbolInfo } from '../alSymbolInfo';

export class ALObjectLibrariesCollection {
    
    private librariesCache : ALObjectLibrary[] = [];

    constructor() {
    }

    async getLibrary(filePath : string, forceReload : boolean) : Promise<ALObjectLibrary> {
        var library : ALObjectLibrary = this.findLibrary(filePath);
        var append : boolean = (!library);
        if (!library)
            library = new ALObjectLibrary();
        await library.loadFromAppFile(filePath, forceReload);
        if (append)
            this.librariesCache.push(library);        
        return library;
    }

    private findLibrary(filePath : string) : ALObjectLibrary {
        for (var i=0; i<this.librariesCache.length; i++) {
            if (this.librariesCache[i].sourceFilePath == filePath)
                return this.librariesCache[i];
        }
        return null;
    }

   async getBasicLibrary(filePath : string, forceReload : boolean) : Promise<ALBasicLibrary> {
        var library : ALObjectLibrary = await this.getLibrary(filePath, forceReload);
        if (library)
            return library.basicLibrary;
        return null;
    }

    findObjectUri(objectType : string, objectId : number) : vscode.Uri {
        for (var i=0; i<this.librariesCache.length; i++) {
            var objectUri : vscode.Uri = this.librariesCache[i].findObjectUri(objectType, objectId);
            if (objectUri != null)
                return objectUri;
        }

        return null;
    }

    findALSymbolInfo(objectType : string, objectId : number) : ALSymbolInfo {
        for (var i=0; i<this.librariesCache.length; i++) {
            var symbolInfo = this.librariesCache[i].findALSymbolInfo(objectType, objectId);
            if (symbolInfo != null)
                return symbolInfo;
        }
        return null;
    }

}