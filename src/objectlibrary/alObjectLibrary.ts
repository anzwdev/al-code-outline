'use strict';

import * as vscode from 'vscode';
import { Buffer } from 'buffer';
import { ALBasicLibrary } from "./alBasicLibrary";
import { IALSymbolInfoDictionary } from "./alSymbolInfoDictionary";
import { ALSymbolInfo } from '../alSymbolInfo';
import { ALSymbolKind } from '../alSymbolKind';

export class ALObjectLibrary {
    sourceFilePath : string;
    sourceSize : number;
    sourceDateTimeMs : number;

    appId : string;
    name : string;
    publisher : string;
    version : string;
    basicLibrary : ALBasicLibrary;

    alSymbolReferences = {} as IALSymbolInfoDictionary;

    constructor() {
    }

    async loadFromAppFile(filePath : string, forceReload : boolean) {
        await vscode.window.withProgress({
			location: vscode.ProgressLocation.Notification,
			title: "Opening application package.",
			cancellable: false
		}, (progress, token) => {
            progress.report({ increment : 0 });
            
            //var p = new Promise(resolve => {
            return this.loadFromAppFileWithProgress(progress, filePath, forceReload);
            //    resolve();
            //});
            
            //return p;
        });
    }

    protected async loadFromAppFileWithProgress(progress : any, filePath : string, forceReload : boolean) {
        var fs = require('fs');
        var AdmZip = require('adm-zip');

        //load zip file into memory
        var offset = 40;

        var stats = fs.statSync(filePath);

        //check if file should be reloaded
        if ((this.sourceFilePath === filePath) && (!forceReload) && (stats.size === this.sourceSize) && (stats.mtimeMs === this.sourceDateTimeMs))
            return;

        progress.report({ increment : 10, message : 'Extracting symbols from the application package.'});

        this.sourceFilePath = filePath;
        this.sourceSize = stats.size;
        this.sourceDateTimeMs = stats.mtimeMs;

        var fileSizeInBytes = stats.size - offset;
        var buffer : Buffer = new Buffer(fileSizeInBytes);

        var fileDesc = fs.openSync(filePath, 'r');
        fs.readSync(fileDesc, buffer, 0, fileSizeInBytes, offset);
        fs.closeSync(fileDesc);

        //load symbol references json from zip file
        var zip = new AdmZip(buffer);      
        var zipEntry = zip.getEntry('SymbolReference.json');
        var jsonObjectList  = zip.readAsText(zipEntry).trim();

        progress.report({ increment : 40, message : 'Loading extracted symbols.'});        

        //parse symbol references
        var symbolReferences = JSON.parse(jsonObjectList);

        progress.report({ increment : 80, message : 'Processing loaded symbols.'});

        this.appId = symbolReferences.AppId;
        this.name = symbolReferences.Name;
        this.publisher = symbolReferences.Publisher;
        this.version = symbolReferences.Version;
        this.basicLibrary = new ALBasicLibrary(this.name, symbolReferences);    //creates basic library and assigns negative Ids to symbol references without them

        //build symbol dictionaries        
        this.alSymbolReferences["Table"] = this.buildObjectSymbolInfoList(ALSymbolKind.Table, "Table", symbolReferences.Tables);
        this.alSymbolReferences["Page"] = this.buildObjectSymbolInfoList(ALSymbolKind.Page, "Page", symbolReferences.Pages);
        this.alSymbolReferences['Report'] = this.buildObjectSymbolInfoList(ALSymbolKind.Report, "Report", symbolReferences.Reports);
        this.alSymbolReferences['XmlPort'] = this.buildObjectSymbolInfoList(ALSymbolKind.XmlPort, "XmlPort", symbolReferences.XmlPorts);
        this.alSymbolReferences['Query'] = this.buildObjectSymbolInfoList(ALSymbolKind.Query, "Query", symbolReferences.Queries);
        this.alSymbolReferences["Codeunit"] = this.buildObjectSymbolInfoList(ALSymbolKind.Codeunit, "Codeunit", symbolReferences.Codeunits);
        this.alSymbolReferences['ControlAddIn'] = this.buildObjectSymbolInfoList(ALSymbolKind.ControlAddIn, "Control AddIn", symbolReferences.ControlAddIns);
        this.alSymbolReferences['PageExtension'] = this.buildObjectSymbolInfoList(ALSymbolKind.PageExtension, "Page Extension", symbolReferences.PageExtensions);
        this.alSymbolReferences['TableExtension'] = this.buildObjectSymbolInfoList(ALSymbolKind.TableExtension, "Table Extension", symbolReferences.TableExtensions);
        this.alSymbolReferences['Profile'] = this.buildObjectSymbolInfoList(ALSymbolKind.Profile, "Profile", symbolReferences.Profiles);
        this.alSymbolReferences['PageCustomization'] = this.buildObjectSymbolInfoList(ALSymbolKind.PageCustomization, "Page Customization", symbolReferences.PageCustomizations);
        this.alSymbolReferences['DotNetPackage'] = this.buildObjectSymbolInfoList(ALSymbolKind.DotNetPackage, "DotNet Package", symbolReferences.DotNetPackages);
        this.alSymbolReferences['Enum'] = this.buildObjectSymbolInfoList(ALSymbolKind.Enum, "Enum", symbolReferences.EnumTypes);
    }

    findObjectUri(objectType : string, objectId : number) : vscode.Uri {
        var objectHeader = this.basicLibrary.findObjectHeader(objectType, objectId);
        if (objectHeader) {
            return vscode.Uri.parse(encodeURI(
                //'al-preview://dynamics/' + objectType + '/' + encodeURIComponent(this.name) + '/' + encodeURIComponent(objectHeader.Id.toString()) + '/' + encodeURIComponent(objectHeader.Name) + '.dal'));
                'al-preview://AlLang/' + encodeURIComponent(this.name) + '/' + objectType + '/' + encodeURIComponent(objectHeader.Id.toString()) + '/' + encodeURIComponent(objectHeader.Name) + '.dal'));
        }
        return null;
    }

    findALSymbolInfo(objectType : string, objectId : number) : ALSymbolInfo {
        var symbolInfoList : ALSymbolInfo[] = this.alSymbolReferences[objectType];
        if (symbolInfoList)
            return symbolInfoList[objectId];        
        return null;
    }

    private buildObjectSymbolInfoList(newSymbolKind : ALSymbolKind, newTypeName : string, refList : any[]) : ALSymbolInfo[] {
        let symbolList : ALSymbolInfo[] = [];
        if (refList) {        
            for (var i=0; i<refList.length; i++) {
                var symbolInfo = new ALSymbolInfo(null, null);
                symbolInfo.loadFromObjectSymbolReference(newSymbolKind, newTypeName, refList[i]);
                symbolList[refList[i].Id] = symbolInfo;
            }
        }
        return symbolList;
    }

}