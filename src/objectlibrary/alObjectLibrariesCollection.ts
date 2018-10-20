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

    protected async runGoToDefinitionOnAlFile(progress : any, sourceCode : string, posLine : number, posColumn : number, lastSourceLine : number, lastSourceColumn : number) {
        if ((!vscode.workspace.workspaceFolders) || (vscode.workspace.workspaceFolders.length == 0))
            return;
        let fs = require('fs');
        let tempFileUri = vscode.Uri.file(path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, ".allangtemp", "tempal.al"));
        let tempFolderUri = path.join(vscode.workspace.workspaceFolders[0].uri.fsPath, ".allangtemp");
        if (!fs.existsSync(tempFolderUri))
            fs.mkdirSync(tempFolderUri);

        progress.report({ increment: 0, message: "Preparing object reference" });

        //create file
        let edit = new vscode.WorkspaceEdit();
        edit.createFile(tempFileUri, {overwrite : true});
        await vscode.workspace.applyEdit(edit);

        //write content to the file
        edit = new vscode.WorkspaceEdit();
        edit.insert(tempFileUri, new vscode.Position(0, 0), sourceCode);
        await vscode.workspace.applyEdit(edit);

        progress.report({ increment: 33, message: "Downloading object definition" });
        
        //download document symbols
        let pos = new vscode.Position(posLine, posColumn);
        let list : any = await vscode.commands.executeCommand('vscode.executeDefinitionProvider', tempFileUri, pos);

        //clear file to remove errors from the workspace
        edit = new vscode.WorkspaceEdit();
        edit.delete(tempFileUri, new vscode.Range(0, 0, lastSourceLine, lastSourceColumn));
        await vscode.workspace.applyEdit(edit);

        /*
        edit = new vscode.WorkspaceEdit();
        edit.deleteFile(tempFileUri, {ignoreIfNotExists : true});        
        await vscode.workspace.applyEdit(edit);
        */
       
        progress.report({ increment: 66, message: "Opening object definition" });

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

    protected async goToAlObjectDefinition(progress : any, objectType : string, objectName : string) {
        objectName = '"' + objectName.replace('"', '""') + '"';       
        let sourceCode : string = 'codeunit 0 "ALLangServerProxy" \n' +
            '{\n' +
            'var\n' +
            'a : ' + objectType + ' ' +  objectName + ';\n' +
            '}\n';
        await this.runGoToDefinitionOnAlFile(progress, sourceCode, 3, objectType.length + 6, 5, 0);
    }

    protected async goToDefinitionAsync(progress : any, objectType : string, objectId : number) {
        //find object name
        let symbolInfo : ALSymbolInfo = this.findALSymbolInfo(objectType, objectId);
        if (symbolInfo == null)
            return;
        //translate object type to variable type
        objectType = objectType.toLowerCase();
        if (objectType == "table")
            objectType = "record";
        
        await this.goToAlObjectDefinition(progress, objectType, symbolInfo.symbolName);       
    }

    goToDefinition(objectType : string, objectId : number) {       
        let enabled : boolean = vscode.workspace.getConfiguration('alOutline').get('enableLanguageServerProxy');
        
        if (!enabled) {
            vscode.window.showWarningMessage("Go to definition can only be invoked from al source code. Please switch 'alOutline.enableLanguageServerProxy' setting in VS Code settings to true, to allow AL Object Browser to insert temporary, empty al file into your project and invoke 'go to definition' command from there.", { modal: true });
            return;
        }
        
        vscode.window.withProgress({
			location: vscode.ProgressLocation.Notification,
			title: "Loading object definition",
			cancellable: false
		}, (progress, token) => {
			progress.report({ increment: 0 });
            return this.goToDefinitionAsync(progress, objectType, objectId);            
        });
    }

}