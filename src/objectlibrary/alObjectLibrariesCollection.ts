import * as vscode from 'vscode';
import * as path from 'path';
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

    protected async runGoToDefinitionOnAlFile(sourceCode : string, posLine : number, posColumn : number, lastSourceLine : number, lastSourceColumn : number) {
        if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
            return;
        let fs = require('fs');
        let tempFileUri = vscode.Uri.file(path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, ".allangtemp", "tempal.al"));
        let tempFolderUri = path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, ".allangtemp");
        if (!fs.existsSync(tempFolderUri))
            fs.mkdirSync(tempFolderUri);

        //create file
        let edit = new vscode.WorkspaceEdit();
        edit.createFile(tempFileUri, {overwrite : true});
        await vscode.workspace.applyEdit(edit);

        //write content to the file
        edit = new vscode.WorkspaceEdit();
        edit.insert(tempFileUri, new vscode.Position(0, 0), sourceCode);
        await vscode.workspace.applyEdit(edit);
        
        //download document symbols
        let pos = new vscode.Position(posLine, posColumn);
        let list : any = await vscode.commands.executeCommand('vscode.executeDefinitionProvider', tempFileUri, pos);

        //clear file to remove errors from the workspace
        edit = new vscode.WorkspaceEdit();
        edit.delete(tempFileUri, new vscode.Range(0, 0, lastSourceLine, lastSourceColumn));
        await vscode.workspace.applyEdit(edit);

        edit = new vscode.WorkspaceEdit();
        edit.deleteFile(tempFileUri, {ignoreIfNotExists : true});
        await vscode.workspace.applyEdit(edit);

        //go to definition
        if ((list) && (list.length > 0)) {
            let location : vscode.Location = list[0];

            vscode.workspace.openTextDocument(location.uri).then(
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

    protected goToAlObjectDefinition(objectType : string, objectName : string) {
        objectName = '"' + objectName.replace('"', '""') + '"';       
        let sourceCode : string = 'codeunit 0 "ALLangServerProxy" \n' +
            '{\n' +
            'var\n' +
            'a : ' + objectType + ' ' +  objectName + ';\n' +
            '}\n';
        this.runGoToDefinitionOnAlFile(sourceCode, 3, objectType.length + 6, 5, 0);
    }

    goToDefinition(objectType : string, objectId : number) {       
        
        //find object name
        let symbolInfo : ALSymbolInfo = this.findALSymbolInfo(objectType, objectId);
        if (symbolInfo == null)
            return;
        //translate object type to variable type
        objectType = objectType.toLowerCase();
        if (objectType == "table")
            objectType = "record";
        
        this.goToAlObjectDefinition(objectType, symbolInfo.symbolName);       
        
        /*
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
        */
        
    }

}