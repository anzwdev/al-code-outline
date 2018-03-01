import * as vscode from 'vscode';
import { ALObjectLibrary } from "./alObjectLibrary";
import { ALBasicLibrary } from "./alBasicLibrary";
import { ALSymbolInfo } from '../alSymbolInfo';

export class ALObjectLibrariesCollection {
    
    private librariesCache : ALObjectLibrary[] = [];

    constructor() {
    }

    getLibrary(filePath : string, forceReload : boolean) {
        var library : ALObjectLibrary = this.findLibrary(filePath);
        var append : boolean = (!library);
        if (!library)
            library = new ALObjectLibrary();
        library.loadFromAppFile(filePath, forceReload);
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

    getBasicLibrary(filePath : string, forceReload : boolean) : ALBasicLibrary {
        var library : ALObjectLibrary = this.getLibrary(filePath, forceReload);
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

    goToDefinition(objectType : string, objectId : number) {
        var uri = this.findObjectUri(objectType, objectId);
        if (uri) {
            vscode.workspace.openTextDocument(uri).then(
                document => { 
                    vscode.window.showTextDocument(document, {
                        preview : false
                    });
                },
                err => {
                    vscode.window.showErrorMessage(err);
                });
        }
    }

}